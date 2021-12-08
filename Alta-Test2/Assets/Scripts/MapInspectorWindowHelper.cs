using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public partial class MapEditor
    {
        private WindowMainLayout m_mainLayout;

        enum WindowMainLayout
        {
            NoData,
            NewMap,
            LoadMap,
            UnsavedMap,
            OnMapLoaded
        }

        public void DisplayWindow()
        {
            switch (m_mainLayout)
            {
                case WindowMainLayout.NoData:

                    if (GUILayout.Button("Create New Map"))
                    {
                        targetMap.data.mapName = string.Empty;
                        m_mainLayout = WindowMainLayout.NewMap;
                    }
                    if (GUILayout.Button("Load Map"))
                    {
                        m_mainLayout = WindowMainLayout.LoadMap;
                    }
                    if (GUILayout.Button("Load Default Map"))
                    {                        
                        targetMap.Initialize(true, JsonMapTool.LoadMap("AltaDemo"));
                        m_mainLayout = WindowMainLayout.UnsavedMap;
                    }

                    if (targetMap.data.savedMap.Count > 0)
                    {
                        m_mainLayout = WindowMainLayout.UnsavedMap;
                    }


                    break;

                case WindowMainLayout.NewMap:

                    targetMap.data.mapType = (MapData.MapType)EditorGUILayout.EnumPopup(targetMap.data.mapType);

                    EditorGUILayout.LabelField($"Grid Size");
                    targetMap.data.gridSize = EditorGUILayout.IntField(targetMap.data.gridSize);
                    EditorGUILayout.LabelField($"Tile Size");
                    targetMap.data.tileSize = EditorGUILayout.IntField(targetMap.data.tileSize);

                    if (GUILayout.Button("Spawn"))
                    {
                        targetMap.Initialize();
                        m_mainLayout = WindowMainLayout.UnsavedMap;
                    }

                    break;

                case WindowMainLayout.LoadMap:

                    EditorGUILayout.LabelField($"Map Name");
                    targetMap.data.mapName = EditorGUILayout.TextArea(targetMap.data.mapName);

                    if (GUILayout.Button("Load Map"))
                    {
                        var data = JsonMapTool.LoadMap(targetMap.data.mapName);
                        targetMap.Initialize(true, data);
                    }

                    m_mainLayout = WindowMainLayout.UnsavedMap;

                    break;

                case WindowMainLayout.UnsavedMap:

                    EditorGUILayout.HelpBox("Select a Node to Edit with Left Click in the scene view", MessageType.Info);

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Create New Map", EditorStyles.miniButtonLeft))
                    {
                        targetMap.data.mapName = string.Empty;
                        m_mainLayout = WindowMainLayout.NewMap;
                    }
                    if (GUILayout.Button("Load Default Map", EditorStyles.miniButtonMid))
                    {
                        targetMap.Initialize(true, JsonMapTool.LoadMap("AltaDemo"));
                        m_mainLayout = WindowMainLayout.UnsavedMap;
                    }
                    if (GUILayout.Button("Load Map", EditorStyles.miniButtonRight))
                    {
                        var data = JsonMapTool.LoadMap(targetMap.data.mapName);
                        targetMap.Initialize(true, data);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(50);

                    EditorGUILayout.LabelField($"Map Name");
                    targetMap.data.mapName = EditorGUILayout.TextArea(targetMap.data.mapName);

                    if (JsonMapTool.FileExists(targetMap.data.mapName))
                    {
                        EditorGUILayout.HelpBox("Saving Now will update an existing save file, this cannot be undone", MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Saving now will create a new map file", MessageType.Info);
                    }                    

                    if (GUILayout.Button("Save Map"))
                    {
                        JsonMapTool.SaveMap(targetMap.data);
                        AssetDatabase.Refresh();
                    }

                    GUILayout.Space(50);

                    if (GUILayout.Button("Find Path"))
                    {
                        setBlockedMode = false;
                        findPathMode = true;
                    }
                    else if (findPathMode == false)
                    {
                        EditorGUILayout.HelpBox("Select Find Path then 2 nodes to see a top down map", MessageType.Info);
                    }
                    else if (findPathMode == true)
                    {
                        if (pathPoints.Count > 1)
                        {
                            switch (targetMap.data.mapType)
                            {
                                case MapData.MapType.TopDown:
                                    targetMap.path = PathFinding.AStar(targetMap.data, pathPoints[0], pathPoints[1], PathMethods.CardinalDistance, PathMethods.gridAllNeighbours);
                                    break;
                                case MapData.MapType.Platformer:
                                    targetMap.path = PathFinding.AStar(targetMap.data, pathPoints[0], pathPoints[1], PathMethods.CardinalDistance, PathMethods.gridPlatformNeighbours, true);
                                    break;
                            }

                            pathPoints.Clear();
                            findPathMode = false;
                        }
                        EditorGUILayout.HelpBox($"Select {(pathPoints.Count - 2).ToString()[1]} more nodes to see path", MessageType.Info);
                    }

                    break;

                default:
                    break;
            }
        }
    }
}
