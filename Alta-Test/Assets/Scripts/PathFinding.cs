using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PathFinding
    {
        public static List<Tile> AStar(Map map, Tile start, Tile goal, int heuristic)
        {
            //setup
            Tile[] grid = map.Grid;

            List<Tile> openSet = new List<Tile>();
            openSet.Add(start);


            if (start == null || goal == null)
            {
                Debug.LogWarning($" Start is set = {start} // Goal is set = {goal}");
                return null;
            }

            List<Tile> closedSet = new List<Tile>();
            Tile currentNode;
            int breakoutCount = 0;
            while (openSet.Count > 0)
            {
                breakoutCount++;
                openSet.Sort((x, y) => x.fScore.CompareTo(y.fScore)); //sort openset by fcost

                currentNode = openSet[0];


                openSet.RemoveAt(0);
                closedSet.Add(currentNode);

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].isBlocked)
                    {
                        continue;
                    }
                    if (openSet[i].fScore < currentNode.fScore || openSet[i].fScore == currentNode.fScore && openSet[i].Distance(goal) < currentNode.Distance(goal))
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == goal)
                {
                    Debug.Log($"Path Found");

                    return GatherPath(start, goal);
                }

                foreach (var neighbour in currentNode.neighbours)
                {
                    if (neighbour.isBlocked)
                    {
                        Debug.Log($"unwalkable found");
                    }

                    if (neighbour.isBlocked || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    float cost = currentNode.gScore + currentNode.Distance(goal);
                    if (cost < neighbour.gScore || !openSet.Contains(neighbour))
                    {
                        neighbour.gScore = cost;
                        neighbour.hScore = neighbour.Distance(goal);
                        neighbour.previous = currentNode;

                        if (!openSet.Contains(neighbour) && !neighbour.isBlocked)
                        {
                            openSet.Add(neighbour);
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
                currentNode = currentNode.previous;
            }
            path.Reverse();
            return path;
        }

    }
}
