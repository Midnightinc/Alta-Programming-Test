using System.Collections.Generic;

namespace Assets.Scripts
{
    [System.Serializable]
    public class MapData
    {

        public string mapName;

        public int tileSize;
        public int gridSize;
        public List<Tile> savedMap;
        public MapType mapType;

        public int GetIndexFromXY(int x, int y)
        {
            return x * gridSize + y;
        }


        public MapData(MapData data)
        {
            this.mapName =  data.mapName;
            this.tileSize = data.tileSize;
            this.gridSize = data.gridSize;
            this.savedMap = data.savedMap;
        }

        public enum MapType
        {
            TopDown,
            Platformer
        }
    }
}
