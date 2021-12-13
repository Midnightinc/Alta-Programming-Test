using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "NewTileBrushPallet", menuName = "Map")]
    public class TileBrushPallet : ScriptableObject
    {
        [Tooltip("Prefab spawned on map loading")]
        public GameObject tilePrefab;

        public bool tileBlocked;
    }
}
