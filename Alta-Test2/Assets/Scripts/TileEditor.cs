using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
#if UNITY_EDITOR

    class TileEditor : EditorWindow
    {

        private Tile m_tile;
        public MapEditor mapEditor;

        private TileBrushPallet pallet;

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
            EditorGUILayout.LabelField("Selected Settings", EditorStyles.boldLabel);
            if (m_tile != null)
            {
                DrawWindow();
            }


        }

        private void DrawWindow()
        {
            EditorGUILayout.HelpBox($"               Node X{m_tile.gridX} - : - Y{m_tile.gridY} Selected", MessageType.Info);

            if (mapEditor)
            {
                EditorGUI.BeginChangeCheck();
                switch (mapEditor.targetMap.data.mapType)
                {
                    case MapData.MapType.TopDown:

                        m_tile.data.DrawEditorInfo();

                        GUILayout.Space(50);

                        if (GUILayout.Button($"{(mapEditor.setBlockedMode ? "Disable Set Block Mode" : "Enable Set Block Mode on all selected")}"))
                        {
                            mapEditor.setBlockedMode = !mapEditor.setBlockedMode;
                        }

                        DrawBrushPalletSettings();

                        if (EditorGUI.EndChangeCheck())
                        {
                            mapEditor.targetMap.onTileModified.Invoke(mapEditor.GetSelectedTile());
                        }
                        break;

                    case MapData.MapType.Platformer:

                        if (EditorGUI.EndChangeCheck())
                        {
                            mapEditor.targetMap.onTileModified.Invoke(mapEditor.GetSelectedTile());
                        }
                        DrawBrushPalletSettings();
                        break;
                }
            }
        }

        private void DrawBrushPalletSettings()
        {
            pallet = EditorGUILayout.ObjectField("Brush Pallet", pallet, typeof(TileBrushPallet), true) as TileBrushPallet;

            GUILayout.BeginHorizontal();

            if (pallet == null)
            {
                if (GUILayout.Button($"Create New Brush", EditorStyles.miniButtonMid))
                {
                    TileBrushPallet newPallet = ScriptableObject.CreateInstance<TileBrushPallet>();

                    AssetDatabase.CreateAsset(newPallet, "Assets/New Pallet Asset.asset");
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                if (GUILayout.Button($"{(mapEditor.enableTileBrush ? "Disable Tile Brush" : "Enable Tile Brush")}", EditorStyles.miniButtonLeft))
                {
                    mapEditor.enableTileBrush = !mapEditor.enableTileBrush;
                }
                if (GUILayout.Button("Fill all empty Nodes", EditorStyles.miniButtonRight))
                {
                    foreach (var tile in mapEditor.targetMap.data.savedMap)
                    {
                        if (tile.data.gameObject == null)
                        {
                            tile.data = TileBrush();
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        public TileData TileBrush()
        {
            if (!pallet)
            {
                return null;
            }
            return new TileData()
            {
                gameObject = pallet.tilePrefab,
                isBlocked = pallet.tileBlocked
            };
        }



    }
#endif
}
