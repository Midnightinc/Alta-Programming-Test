using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Map currentMap;

    private Transform _transform;
    private Camera Cam;

    private Tile lastMouseOver;
    private Tile lastPathTarget;

    private List<Tile> psuedoPath = new List<Tile>();
    private List<Tile> followPath = new List<Tile>();


    public void TileChangedNotify(Tile tile)
    {
        print($"OnTileModified: The tile at {tile.gridX} - {tile.gridY} has been modified");
    }

    private void Start()
    {
        _transform = transform;
        Vector2 position = _transform.position;
        _transform.position = currentMap.GetMapPosition(position.x,position.y);
        Cam = Camera.current;
    }

    private void Update()
    {
        if (Cam == null)
        {
            Cam = Camera.current;
            return;
        }
        Tile targetUnderMouse = GetTileUnderMouse();

        if (targetUnderMouse != lastMouseOver)
        {
            lastMouseOver = targetUnderMouse;

            psuedoPath = GetPathToTile(targetUnderMouse);

        }
        if (Input.GetMouseButtonDown(0) && targetUnderMouse != lastPathTarget)
        {
            followPath = GetPathToTile(targetUnderMouse);
            lastPathTarget = targetUnderMouse;
        }
    }

    private void FixedUpdate()
    {
        if (followPath != null && followPath.Count > 0)
        {
            Vector2 position = currentMap.GetMapPosition(followPath[0].InverseWorldPosition.x, followPath[0].InverseWorldPosition.y);

            float distance = Vector3.Distance(_transform.position, position);
            _transform.position = Vector3.Lerp(_transform.position, position, distance);

            if (distance < .1f)
            {
                followPath.RemoveAt(0);
            }
        }
    }

    private Tile GetTileUnderMouse()
    {

        var position = Cam.ScreenToWorldPoint(Input.mousePosition);
        return currentMap.GetMapTile((int)position.y, (int)position.x);
        
    }

    private List<Tile> GetPathToTile(Tile tile)
    {
        if (Cam == null)
        {
            Cam = Camera.current;
        }

        var location = currentMap.GetMapTile((int)_transform.position.y, (int)_transform.position.x);


        return PathFinding.AStar(currentMap.data, location, tile, PathMethods.CardinalDistance, PathMethods.gridAllNeighbours);
    }

    private void OnDrawGizmos()
    {
        if (psuedoPath != null && psuedoPath.Count > 0)
        {
            for (int i = 0; i < psuedoPath.Count; i++)
            {
                currentMap.DrawPath(psuedoPath, Color.magenta);
            }
        }
        if (followPath != null && followPath.Count > 0)
        {
            for (int i = 0; i < followPath.Count; i++)
            {
                currentMap.DrawPath(followPath, Color.black);
            }
        }
    }
}
