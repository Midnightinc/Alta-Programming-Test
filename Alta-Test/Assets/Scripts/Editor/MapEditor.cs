using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Editor;

namespace Assets.Scripts
{


#if UNITY_EDITOR

    [CustomEditor(typeof(Map))]
    public class MapEditor : UnityEditor.Editor
    {



        private Map m_targetMap;

        private bool editorAutoSave = false;

        private int m_oldIndex = -1;

        private int mouseDownCoord;
        private bool mouseWasDown = false;
        public bool setBlockedMode;

        private void OnEnable()
        {
            m_targetMap = target as Map;
        }

        private void OnSceneGUI()
        {
            m_targetMap = target as Map;
            if (m_targetMap == null)
            {
                return;
            }


            //if (Event.current != null)
            //{
            var position = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(Event.current.mousePosition);

            //This makes the mouse position adjust so it is relative to the world position
            position.y = -(position.y - (2 * SceneView.currentDrawingSceneView.camera.transform.position.y));
            // Debug.Log($"position: {position}");

            position /= m_targetMap.tileSize;

            var index = m_targetMap.Index((int)position.x, (int)position.y);

            if (!(index >= 0 && index < m_targetMap.Grid.Length))
            {
                index = -1;
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                if (index != m_targetMap.selectedIndex)
                {
                    m_oldIndex = index;
                    m_targetMap.selectedIndex = index;
                    SceneView.currentDrawingSceneView.Repaint();

                    if (setBlockedMode)
                    {
                        m_targetMap.Grid[index].isBlocked = true;
                    }

                    TileEditor window = (TileEditor)EditorWindow.GetWindow(typeof(TileEditor));
                    window.Show();
                    Debug.Log($"Error point - selected = {m_targetMap.selectedIndex} : length = {m_targetMap.Grid.Length} : size = {m_targetMap.gridSize}");
                    window.SetTile(m_targetMap.Grid[m_targetMap.selectedIndex]);
                    mouseDownCoord = index;

                }
                return;
            }



            if (index != m_oldIndex)
            {
                m_oldIndex = index;
                m_targetMap.highlightedIndex = index;
                SceneView.currentDrawingSceneView.Repaint();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            EditorGUILayout.BeginHorizontal();              // Start Horizontal Group
            EditorGUILayout.LabelField($"Auto Save");
            editorAutoSave = EditorGUILayout.Toggle(editorAutoSave);
            EditorGUILayout.EndHorizontal();               // End Horizontal Group


            EditorGUILayout.LabelField($"Map Name");
            m_targetMap.mapName = EditorGUILayout.TextArea(m_targetMap.mapName);

            EditorGUILayout.LabelField($"Grid Size");
            m_targetMap.gridSize = EditorGUILayout.IntField(m_targetMap.gridSize);
            
            EditorGUILayout.LabelField($"Tile Size");
            m_targetMap.tileSize = EditorGUILayout.IntField(m_targetMap.tileSize);

            if (GUILayout.Button("Initialize Grid"))
            {
                m_targetMap.Initialize();
            }

            if (GUILayout.Button("Save Map"))
            {
                SaveMapData(m_targetMap);
            }

            if (GUILayout.Button("Load Map"))
            {
                LoadMapData();
            }

            if (GUILayout.Button($"{(setBlockedMode ? "Disable blocked mode" : "Enable blocked Mode")}"))
            {
                setBlockedMode = !setBlockedMode;
            }

            if (GUILayout.Button("Find Path"))
            {
                if (m_targetMap.selectedIndex > 0 && m_targetMap.selectedIndex < m_targetMap.Grid.Length)
                {
                    m_targetMap.FindPath(m_targetMap.Grid[m_targetMap.selectedIndex]);

                }
            }
        }

        private void LoadMapData()
        {
            m_targetMap.Grid = null;
            m_targetMap.gridSize = (int)Mathf.Sqrt(m_targetMap.data.SavedMap.Length);
            m_targetMap.Grid = m_targetMap.data.SavedMap;
            m_targetMap.tileSize = m_targetMap.data.tileSize;
        }

        private void SaveMapData(Map map)
        {
            if (map.data == null)
            {
                map.data = ScriptableObject.CreateInstance<MapData>();
                AssetDatabase.CreateAsset(map.data, $"Assets/{map.mapName}Data.asset");
                EditorUtility.SetDirty(map.data);
            }
            else
            {
                map.data.SetData(map);
                EditorUtility.SetDirty(map.data);
            }
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
