using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.PathFinding;

namespace Assets.Scripts
{
    public static class PathMethods
    {

        public static Heuristic CardinalDistance => new Heuristic((x, y) => Distance(x, y));
        public static GetNeighbours gridAllNeighbours => new GetNeighbours((m, x, y) => GetNeighboursNSEW(m, x, y));
        public static GetNeighbours gridPlatformNeighbours => new GetNeighbours((m, x, y) => GetNeighboursPlatform(m, x, y));



        private static float Distance(Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a, b);
        }

        private static List<Tile> GetNeighboursPlatform(MapData data, int x, int y)
        {
            Tile origin = data.savedMap[data.GetIndexFromXY(x, y)];
            List<Tile> neighbours = new List<Tile>();

            for (int xN = -1; xN <= 1; xN++)
            {
                for (int yN = -1; yN <= 1; yN++)
                {
                    if (xN == 0 && yN == 0)
                    {
                        continue;
                    }

                }
            }

            return neighbours;
        }


        private static List<Tile> GetNeighboursAt(MapData data, int x, int y)
        {
            Tile origin = data.savedMap[data.GetIndexFromXY(x, y)];
            List<Tile> neighbours = new List<Tile>();

            for (int xN = -1; xN <= 1; xN++)
            {
                for (int yN = -1; yN <= 1; yN++)
                {
                    if (xN == 0 && yN == 0)
                    {
                        continue;
                    }

                    var searchX = x + xN;
                    var searchY = y + yN;

                    var index = data.GetIndexFromXY(searchX, searchY);
                    if (index < 0 || index > data.savedMap.Count - 1)
                    {
                        continue;
                    }
                    var neighbour = data.savedMap[index];

                    if (neighbour != null && Vector2.Distance(origin.WorldPosition, neighbour.WorldPosition) < (data.tileSize * 2))
                    {
                        neighbours.Add(neighbour);
                    }
                }
            }

            return neighbours;
        }

        /// <summary>
        /// Get neighbours from only north south east and west
        /// </summary>
        /// <param name="data"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static List<Tile> GetNeighboursNSEW(MapData data, int x, int y)
        {
            Tile origin = data.savedMap[data.GetIndexFromXY(x, y)];
            List<Tile> neighbours = new List<Tile>();

            for (int xN = -1; xN <= 1; xN+=2)
            {
                var searchX = x + xN;

                var index = data.GetIndexFromXY(searchX, y);
                if (index < 0 || index > data.savedMap.Count - 1)
                {
                    continue;
                }
                var neighbour = data.savedMap[index];

                if (neighbour != null && Vector2.Distance(origin.WorldPosition, neighbour.WorldPosition) < (data.tileSize * 2))
                {
                    neighbours.Add(neighbour);
                }
            }
            for (int yN = -1; yN <= 1; yN+=2)
            {
                var searchY = y + yN;

                var index = data.GetIndexFromXY(x, searchY);
                if (index < 0 || index > data.savedMap.Count - 1)
                {
                    continue;
                }
                var neighbour = data.savedMap[index];

                if (neighbour != null && Vector2.Distance(origin.WorldPosition, neighbour.WorldPosition) < (data.tileSize * 2))
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }
    }
}
