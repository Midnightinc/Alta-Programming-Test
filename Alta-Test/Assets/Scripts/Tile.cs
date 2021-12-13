using UnityEngine;

namespace Assets.Scripts
{
    public class PathPointData
    {
        public Tile previous;
        public float gScore;
        public float hScore;
        public float fScore => gScore + hScore;
    }



    [System.Serializable]
    public class Tile
    {
        //world position of bottom left point on tile
        public Vector2 InverseWorldPosition => new Vector2(gridY * tileSize, gridX * tileSize);

        public Vector2 WorldPosition => new Vector2(gridX * tileSize, gridY * tileSize);
        private int tileSize;

        public int gridX, gridY; // grid coordinates

        public TileData data;
        public PathPointData pathData;

        public bool isHighlighted;
        public bool isSelected;

        public Tile(int gridX, int gridY, int tileSize)
        {
            this.gridX = gridX;
            this.gridY = gridY;
            this.tileSize = tileSize;
            data = new TileData();
            pathData = new PathPointData();
        }



        #region Gizmos Draw Methods
        public void DrawSolid(Color color)
        {
            Gizmos.color = color;
            Vector3 position = new Vector3(WorldPosition.y + (tileSize * .5f), WorldPosition.x + (tileSize * .5f));
            Gizmos.DrawCube(position, new Vector3(tileSize, tileSize, 0));
        }

        public void DrawOutline(Color color)
        {
            Gizmos.color = color;

            Vector3 from, to;
            from = InverseWorldPosition;
            to = InverseWorldPosition;

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
        #endregion

    }
}
