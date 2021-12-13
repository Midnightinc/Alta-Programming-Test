using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class TileData
    {

        public Color gizmoColor;

        public GameObject gameObject;
        public bool isBlocked;
        public string gameObjectName;


#if UNITY_EDITOR
        public void DrawEditorInfo()
        {
            isBlocked = EditorGUILayout.ToggleLeft("Tile Is Blocked", isBlocked);


            gameObject = EditorGUILayout.ObjectField("Tile Prefab to instantiate on load", gameObject, typeof(GameObject), false) as GameObject;
        }
#endif
    }


}
