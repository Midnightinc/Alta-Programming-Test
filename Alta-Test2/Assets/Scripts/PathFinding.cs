using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PathFinding
    {

        public delegate float Heuristic(Vector2 x, Vector2 y);
        public delegate List<Tile> GetNeighbours(MapData map, int x, int y);



        public static List<Tile> AStar(MapData map, Tile start, Tile goal, Heuristic heuristic, GetNeighbours neighbourDelegate, bool platformer = false)
        {

            List<Tile> openSet = new List<Tile>();
            openSet.Add(start);


            if (start == null || goal == null)
            {
                return null;
            }

            if (start.data.isBlocked || goal.data.isBlocked)
            {
                return null;
            }

            List<Tile> closedSet = new List<Tile>();
            Tile currentNode;
            int breakoutCount = 0;
            while (openSet.Count > 0)
            {
                breakoutCount++;
                //sort openset by fcost
                openSet.Sort((x, y) => x.pathData.fScore.CompareTo(y.pathData.fScore));

                currentNode = openSet[0];

                //get first node to exaluate
                openSet.RemoveAt(0);
                closedSet.Add(currentNode);

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].data.isBlocked)
                    {
                        continue;
                    }
                    if (heuristic(openSet[i].WorldPosition, goal.WorldPosition) < heuristic(openSet[i].WorldPosition, goal.WorldPosition))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == goal)
                {
                    return GatherPath(start, goal);
                }

                foreach (var neighbour in neighbourDelegate(map, currentNode.gridX, currentNode.gridY))
                {

                    if (neighbour.data.isBlocked || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    if (currentNode.pathData == null)
                    {
                        Debug.Log($"currentNode.pathData null?");
                    }

                    float cost = currentNode.pathData.gScore + heuristic(currentNode.WorldPosition, goal.WorldPosition);
                    if (cost < neighbour.pathData.gScore || !openSet.Contains(neighbour))
                    {
                        neighbour.pathData.gScore = cost;
                        neighbour.pathData.hScore = heuristic(neighbour.WorldPosition, goal.WorldPosition);
                        neighbour.pathData.previous = currentNode;

                        if (platformer)
                        {
                            if (!openSet.Contains(neighbour) && neighbour.data.isBlocked)
                            {
                                openSet.Add(neighbour);
                            }
                        }
                        else
                        {
                            if (!openSet.Contains(neighbour) && !neighbour.data.isBlocked)
                            {
                                openSet.Add(neighbour);
                            }
                        }

                    }
                }
            }
            return null;
        }



        private static List<Tile> GatherPath(Tile start, Tile end)
        {
            List<Tile> path = new List<Tile>();
            Tile currentNode = end;

            while (currentNode != start)
            {
                path.Add(currentNode);
                currentNode = currentNode.pathData.previous;
            }
            path.Reverse();
            return path;
        }

    }
}
