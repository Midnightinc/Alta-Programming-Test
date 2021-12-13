using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public partial class MapEditor
    {
        private WindowMainLayout m_mainLayout;
        private int m_mapLoadIndex = 0;
        private bool m_autoSave;

        enum WindowMainLayout
        {
            NewMap,
            UnsavedMap
        }

        public void DisplayWindow()
        {

            switch (m_mainLayout)
            {
                case WindowMainLayout.NewMap:

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
                        JsonMapTool.GetAllMapNames();
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

                    /*
                     * Get all save map names and create a dropdown to choose between them 
                     */
                    string[] mapArray = JsonMapTool.GetAllMapNames();
                    EditorGUILayout.LabelField($"Select Map Name");
                    int newSelection = EditorGUILayout.Popup(m_mapLoadIndex, mapArray);
                    if (m_mapLoadIndex != newSelection)
                    {
                        m_mapLoadIndex = newSelection;
                        targetMap.data.mapName = mapArray[m_mapLoadIndex];
                    }


                    if (GUILayout.Button($"{(m_autoSave ? "Disable Autosave" : "Enable Autosave")}"))
                    {
                        m_autoSave = !m_autoSave;
                        SubscribeToAutosave(!m_autoSave);
                    }


                    /*
                     * Create a field to enter a unique name for a map
                     * 
                     * create a dialogue help box outlining if the name entered is unique or if saving will overwrite a saved map
                     */
                    EditorGUILayout.LabelField($"Enter New Name");
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


                    /*
                     Find a path through the next two selected nodes
                     */
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
                            targetMap.path = PathFinding.AStar(targetMap.data, pathPoints[0], pathPoints[1], PathMethods.CardinalDistance, PathMethods.gridAllNeighbours);

                            pathPoints.Clear();
                            findPathMode = false;
                        }
                        EditorGUILayout.HelpBox($"Select {(pathPoints.Count - 2).ToString()[1]} more nodes to see path", MessageType.Info);
                    }

                    break;

                default:
                    m_mainLayout = WindowMainLayout.NewMap;
                    break;
            }
        }
    }
}
