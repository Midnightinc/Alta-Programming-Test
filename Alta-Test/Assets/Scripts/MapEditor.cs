#if UNITY_EDITOR
using Assets.Scripts.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    [CustomEditor(typeof(Map))]
    public partial class MapEditor : UnityEditor.Editor
    {
        public Map targetMap;

        public bool setBlockedMode;
        public bool enableTileBrush;

        private TileEditor tileEditor;

        private Tile m_oldIndex;
        private Tile m_selectedTile;
        private bool findPathMode;
        private List<Tile> pathPoints = new List<Tile>();

        private void OnEnable()
        {
            targetMap = target as Map;
        }

        /// <summary>
        /// Subscribe and unsubscribe to the on modified function 
        /// </summary>
        /// <param name="unsubscribe"></param>
        private void SubscribeToAutosave(bool unsubscribe = false)
        {
            void Save(Tile tile)
            {
                Debug.Log($"Autosaving");
                JsonMapTool.SaveMap(targetMap.data);
            }

            if (unsubscribe == true)
            {
                targetMap.onTileModified.RemoveListener((t) => Save(t));
                return;
            }

            targetMap.onTileModified.AddListener((t) => Save(t));
        }


        private void OnSceneGUI()
        {
            targetMap = target as Map;

            if (targetMap.data.savedMap == null && targetMap.data.mapName != "")
            {
                targetMap.Initialize(true, JsonMapTool.LoadMap(targetMap.data.mapName));
            }

            if (targetMap == null || targetMap.data.savedMap == null || targetMap.data.savedMap.Count < 1)
            {
                return;
            }

            var position = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(Event.current.mousePosition);

            //This makes the mouse position adjust so it is relative to the world position
            position.y = -(position.y - (2 * SceneView.currentDrawingSceneView.camera.transform.position.y));

            position /= targetMap.data.tileSize;

            int xInt = (int)position.x;
            int yInt = (int)position.y;

            if (!(xInt > -1 && xInt < targetMap.data.gridSize && yInt > -1 && yInt < targetMap.data.gridSize))
            {
                return;
            }

            Tile index = targetMap.data.savedMap[targetMap.data.GetIndexFromXY((int)position.y,(int)position.x)];

            if (index != null)
            {
                if (m_oldIndex != null)
                {
                    m_oldIndex.isHighlighted = false;
                }

                index.isHighlighted = true;
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (index != m_selectedTile)
                    {
                        if (m_selectedTile != null && index != m_selectedTile)
                        {
                            m_oldIndex.isSelected = false;
                            m_selectedTile.isSelected = false;
                            m_selectedTile = null;
                        }


                        m_oldIndex = index;
                        index.isSelected = true;
                        m_selectedTile = index;
                        SceneView.currentDrawingSceneView.Repaint();

                        if (enableTileBrush)
                        {
                            index.data = tileEditor.TileBrush();
                            targetMap.onTileModified.Invoke(index);
                        }

                        if (setBlockedMode)
                        {
                            index.data.isBlocked = true;
                            targetMap.onTileModified.Invoke(index);
                        }

                        if (findPathMode)
                        {
                            pathPoints.Add(index);
                        }

                        tileEditor = (TileEditor)EditorWindow.GetWindow(typeof(TileEditor));
                        tileEditor.mapEditor = this;
                        tileEditor.Show();
                        tileEditor.SetTile(index);

                    }
                    return;
                }



                if (index != m_oldIndex)
                {
                    m_oldIndex = index;
                    SceneView.currentDrawingSceneView.Repaint();
                }
            }
        }

        public bool AutosaveEnabled()
        {
            return m_autoSave;
        }

        public Tile GetSelectedTile()
        {
            return m_selectedTile;
        }

        public override void OnInspectorGUI()
        {
            if (m_mainLayout == WindowMainLayout.UnsavedMap)
            {
                base.OnInspectorGUI();
            }
            DisplayWindow();
        }
    }
}

#endif