using System.Collections.Generic;
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

        public List<Tile> neighbours = new List<Tile>();

        public Vector2 WorldPosition => worldPosition;
        public bool isBlocked = false;

        public float gScore;
        public float hScore;
        public float fScore => gScore + hScore;

        public Tile previous;


        public float Distance(Tile goal)
        {
            var pos = worldPosition / tileSize;
            var goalPos = goal.worldPosition / tileSize;
            int dstX = (int)Mathf.Abs(pos.x - goalPos.x);
            int dstY = (int)Mathf.Abs(pos.y - goalPos.y);

            if (dstX < dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            else
            {
                return 14 * dstX + 10 * (dstY - dstX);
            }
        }


        public Tile(Vector2 worldPosition, int index, int tileSize, int gridSize)
        {
            worldPosition.x += (index % gridSize) * tileSize;
            worldPosition.y += (index / gridSize) * tileSize;

            this.worldPosition = worldPosition;
            this.index = index;
            this.tileSize = tileSize;
        }

        public void DrawSolid(Color color)
        {
            Gizmos.color = color;
            Vector3 position = new Vector3(worldPosition.y, worldPosition.x);
            position.x += tileSize / 2;
            position.y += tileSize / 2;
            Gizmos.DrawCube(position, new Vector3(tileSize, tileSize, 0));
        }

        public void DrawOutline(Color color)
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
