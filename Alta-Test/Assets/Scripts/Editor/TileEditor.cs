using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
#if UNITY_EDITOR

    class TileEditor : EditorWindow
    {

        private Tile m_tile;

        public void SetTile(Tile tile)
        {
            m_tile = tile;
        }

        [MenuItem("MapEditor/TileEditor")]
        static void Init()
        {
            TileEditor window = (TileEditor)EditorWindow.GetWindow(typeof(TileEditor));
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Selected Settings", EditorStyles.boldLabel);
            if (m_tile != null)
            {
                EditorGUILayout.LabelField($"Selected Tile = {m_tile}", EditorStyles.boldLabel);


                //toggle tile walkable
                EditorGUILayout.LabelField("Is Tile Walkable");
                m_tile.isBlocked = EditorGUILayout.Toggle(m_tile.isBlocked);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log($"Hit the golden hot spot");
            }
            
        }



    }
#endif
}
