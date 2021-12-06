using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
#if UNITY_EDITOR

    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {
        private bool editorAutoSave = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var map = target as Map;


            EditorGUILayout.BeginHorizontal();              // Start Horizontal Group
            EditorGUILayout.LabelField($"Auto Save");
            editorAutoSave = EditorGUILayout.Toggle(editorAutoSave);
            EditorGUILayout.EndHorizontal();               // End Horizontal Group

            EditorGUILayout.LabelField($"Grid Size");
            map.gridSize = EditorGUILayout.IntField(map.gridSize);
            
            EditorGUILayout.LabelField($"Tile Size");
            map.tileSize = EditorGUILayout.IntField(map.tileSize);

            if (GUILayout.Button("Initialize Grid"))
            {
                map.Initialize();
            }

            if (GUILayout.Button("Save Map"))
            {
                SaveMapData(map);
            }

        }



        private void SaveMapData(Map map)
        {
            EditorUtility.SetDirty(map.data);
        }
    }
#endif
}
