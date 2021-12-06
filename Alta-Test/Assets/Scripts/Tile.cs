using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class Tile
    {
        //world position of bottom left point on tile
        private Vector2 worldPosition;
        private int     tileSize;
        private int     index;

        public Tile(Vector2 worldPosition, int index, int tileSize, int gridSize)
        {
            worldPosition.x += (index % gridSize) * tileSize;
            worldPosition.y += (index / gridSize) * tileSize;

            this.worldPosition = worldPosition;
            this.index = index;
            this.tileSize = tileSize;
        }

        public void DrawGizmo(Color color)
        {
            Gizmos.color = color;

            //initialize gizmo drawing properties
            Vector3 from, to;
            from = worldPosition;
            to = worldPosition;

            to.x += tileSize;
            Gizmos.DrawLine(from, to); // bottom left to bottom right

            from.x += tileSize;
            to.y += tileSize;
            Gizmos.DrawLine(from, to); // bottom right to top right

            from.y += tileSize;
            to.x -= tileSize;
            Gizmos.DrawLine(from, to); // top right to top left

            from.x -= tileSize;
            to.y -= tileSize;
            Gizmos.DrawLine(from, to); // top left to bottom left
        }
    }
}
