using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR

#endif

public class Map : MonoBehaviour
{
    [HideInInspector]
    public List<Tile> path;

    [HideInInspector]
    public MapData data;

    public UnityEvent<Tile> onTileModified = new UnityEvent<Tile>();

    private bool running;

    public Tile GetMapTile(int x, int y)
    {
        int index = data.GetIndexFromXY(x, y);
        if (index > data.savedMap.Count || index < 0)
        {
            return null;
        }
        return data.savedMap[index];
    }

    public Vector2 GetMapPosition(float x, float y)
    {
        Vector2 position = GetMapTile((int)x,(int)y).WorldPosition;
        position.x += (data.tileSize * .5f);
        position.y += (data.tileSize * .5f);
        return position;
    }

    private void OnEnable()
    {
        Initialize(true, JsonMapTool.LoadMap(data.mapName));
    }

    public void Initialize(bool copy = false, MapData _data = null)
    {
        if (copy)
        {
            if (_data == null)
            {
                return;
            }

            data.mapName = _data.mapName;
            data.gridSize = _data.gridSize;
            data.tileSize = _data.tileSize;
        }

        print($"Initializing Map with {data.gridSize * data.gridSize} space");
        data.savedMap = new List<Tile>();
        for (int x = 0; x < data.gridSize; x++)
        {
            for (int y = 0; y < data.gridSize; y++)
            {
                if (copy)
                {
                    var tile = _data.savedMap[data.GetIndexFromXY(x, y)];
                    var newTile = new Tile(x, y, data.tileSize);
                    newTile.data = tile.data;
                    data.savedMap.Add(newTile);
                }
                else
                {
                    data.savedMap.Add(new Tile(x, y, data.tileSize));
                }
            }
        }

    }


    private void Start()
    {
        running = true;
        Initialize(true, JsonMapTool.LoadMap(data.mapName));
        for (int i = 0; i < data.savedMap.Count; i++)
        {
            if (data.savedMap[i].data.gameObject != null)
            {
                Vector3 position = Vector3.zero;
                position.y += (data.savedMap[i].gridX * data.tileSize) + (data.tileSize * 0.5f);
                position.x += (data.savedMap[i].gridY * data.tileSize) + (data.tileSize * 0.5f);
                Instantiate(data.savedMap[i].data.gameObject, position, Quaternion.identity, transform);
            }
        }
    }



    void OnDrawGizmos()
    {

        if (data == null)
        {
            print($"exiting at data null");
            return;
        }

        if (!running)
        {
            if (data.savedMap != null && data.savedMap.Count > 0)
            {
                Tile node;
                for (int x = 0; x < data.gridSize; x++)
                {
                    for (int y = 0; y < data.gridSize; y++)
                    {
                        int index = data.GetIndexFromXY(x, y);
                        if (index >= 0 && index < data.savedMap.Count)
                        {
                            node = data.savedMap[index];
                            if (node.isSelected)
                            {
                                node.DrawSolid(Color.green);
                            }
                            else if (node.isHighlighted)
                            {
                                node.DrawSolid(Color.yellow);
                            }
                            else if (node.data.isBlocked)
                            {
                                node.DrawSolid(Color.red);
                            }
                            else if (node.data.gameObject != null)
                            {
                                node.DrawSolid(Color.white);
                            }
                            else
                            {
                                data.savedMap[data.GetIndexFromXY(x, y)].DrawOutline(Color.cyan);
                            }
                        }
                    }
                }
            }

        }

        if (path != null)
        {
            DrawPath(path, Color.black);
        }
    }

    public void DrawLine(Vector2 pos1, Vector2 pos2, Color color)
    {

        Gizmos.color = color;

        float temp = 0;
        temp = pos1.x + (data.tileSize * 0.5f);
        pos1.x = pos1.y + (data.tileSize * 0.5f);
        pos1.y = temp;

        temp = pos2.x + (data.tileSize * 0.5f);
        pos2.x = pos2.y + (data.tileSize * 0.5f);
        pos2.y = temp;


        Gizmos.DrawLine(pos1, pos2);
    }

    public void DrawPath(List<Tile> toDraw, Color color)
    {
        for (int i = 0; i < toDraw.Count - 1; i++)
        {
            if (i == toDraw.Count - 2)
            {
                toDraw[i + 1].DrawSolid(color);
            }

            Gizmos.color = color;

            DrawLine(toDraw[i].WorldPosition, toDraw[i + 1].WorldPosition, color);
        }
    }


}
