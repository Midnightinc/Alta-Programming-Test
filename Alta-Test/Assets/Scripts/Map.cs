using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Map : MonoBehaviour
    {
        [HideInInspector, Tooltip("Map Name")]
        public string mapName;        
        [HideInInspector, Tooltip("Grid Size")]
        public int gridSize;
        [HideInInspector, Tooltip("Tile Size")]
        public int tileSize;

        private Tile[] m_grid;

        private List<Tile> path;

        public MapData data;

        [HideInInspector] public UnityEvent onMapChanged;
        [HideInInspector] public UnityEvent onTileChanged;

        public Tile[] Grid
        {
            get => m_grid;
            set
            {
                m_grid = value;
            }
        }

        public void FindPath(Tile tile)
        {
            Tile origin = Grid[Index((int)tile.WorldPosition.x / tileSize, (int)tile.WorldPosition.y / tileSize)];
            Tile destination = Grid[Index(gridSize / 2, gridSize / 2)];
            path = PathFinding.AStar(this,origin,destination , 5);
        }



        public List<int> highlightedMulti = new List<int>();
        public int highlightedIndex = -1;
        public int selectedIndex = -1;

        public bool displaySavedOnly = false;
        public bool visualizeNeighbours = false;

        private void OnDrawGizmos()
        {
            if (m_grid != null)
            {
                if (displaySavedOnly)
                {
                    for (int i = 0; i < data.SavedMap.Length; i++)
                    {
                        if (m_grid[i].isBlocked)
                        {
                            m_grid[i].DrawSolid(Color.black); // saved blocked paths
                        }

                        if (highlightedMulti.Contains(i))
                        {
                            m_grid[i].DrawSolid(Color.blue); //highlighted multi
                        }


                        m_grid[i].DrawOutline(Color.green); // saved
                    }
                }
                else
                {
                    for (int i = 0; i < m_grid.Length; i++)
                    {
                        if (m_grid[i].isBlocked)
                        {
                            m_grid[i].DrawSolid(Color.red);
                        }


                        m_grid[i].DrawOutline(Color.cyan);
                    }
                    if (highlightedIndex >= 0 && highlightedIndex < m_grid.Length)
                    {
                        m_grid[highlightedIndex].DrawSolid(Color.yellow); //highlighted
                    }
                    if (selectedIndex >= 0 && selectedIndex < m_grid.Length)
                    {
                        m_grid[selectedIndex].DrawSolid(Color.green);
                        if (visualizeNeighbours)
                        {
                            foreach (var node in m_grid[selectedIndex].neighbours)
                            {
                                node.DrawSolid(Color.yellow);
                            }
                        }
                    }
                }
            }


            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawLine(path[i].WorldPosition, path[i + 1].WorldPosition);
                }
            }
        }

        public int Index(int x, int y)
        {
            return (x * gridSize) + y;
        }

        public void Initialize()
        {
            print($"Initializing Map with {gridSize * gridSize} space");
            m_grid = null;
            m_grid = new Tile[gridSize * gridSize];
            for (int i = 0; i < gridSize * gridSize; i++)
            {
                m_grid[i] = new Tile(transform.position, i, tileSize, gridSize);
            }

            List<Tile> nodes = new List<Tile>();
            for (int i = 0; i < m_grid.Length; i++)
            {
                var node = m_grid[i];

                nodes.Clear();

                // grid x and y values for this node - not world position
                int gx = i / gridSize;
                int gy = i % gridSize;


                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x == 0 && y == 0)
                        {
                            continue;
                        }

                        var trueX = gx + x;
                        var trueY = gy + y;

                        var index = Index(trueX, trueY);
                        if (index < Grid.Length && index > 0)
                        {
                            var neighbour = Grid[index];
                            if (!node.neighbours.Contains(neighbour))
                            {
                                node.neighbours.Add(neighbour);
                            }
                        }

                    }
                }
            }
        }
    }
}