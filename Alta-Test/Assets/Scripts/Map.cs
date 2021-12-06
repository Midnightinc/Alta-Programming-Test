using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Map : MonoBehaviour
    {
        [HideInInspector, Tooltip("Grid Size")]
        public int gridSize;
        [HideInInspector, Tooltip("Grid Size")]
        public int tileSize;


        private Tile[] m_grid;

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

        




        private void OnDrawGizmos()
        {
            foreach (var tile in m_grid)
            {
                tile.DrawGizmo(Color.cyan);
            }
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
        }
    }
}