using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance {  get { return _instance; } }
    
    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;

    private void Awake()
    {
        if (_instance != null && _instance != this ){
            Destroy( this.gameObject );
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var tilemap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, OverlayTile>();

        BoundsInt bounds = tilemap.cellBounds;

        //looping through all of our tiles
 //       for (int z = bounds.max.z; z >= bounds.min.z; z--)
 //       {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, 0);
                    var tilekey = new Vector2Int(x, y);

                    if (tilemap.HasTile(tileLocation)) //&& !map.ContainsKey(tilekey))
                    {
                        var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                        var cellWorldPosition = tilemap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 0);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.gridLocation = tileLocation;
                        map.Add(tilekey, overlayTile);
                    }
                }
            }
 //       }
    }

    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles, OverlayTile endTile = null)
    {
        var map = MapManager.Instance.map;

        Dictionary<Vector2Int, OverlayTile> tileToSearch = new Dictionary<Vector2Int, OverlayTile>();

        if(searchableTiles.Count > 0)
        {
            foreach(var item in searchableTiles)
            {
                tileToSearch.Add(item.grid2DLocation, item);
            }
        }else
        {
            tileToSearch = map;
        }


        List<OverlayTile> neighbours = new List<OverlayTile>();

        //top neighbour
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

        if (tileToSearch.ContainsKey(locationToCheck) && (!tileToSearch[locationToCheck].isBlocked || (endTile != null && tileToSearch[locationToCheck] == endTile)))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        //bottom neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

        if (tileToSearch.ContainsKey(locationToCheck) && (!tileToSearch[locationToCheck].isBlocked || (endTile != null && tileToSearch[locationToCheck] == endTile)))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        //right neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);

        if (tileToSearch.ContainsKey(locationToCheck) && (!tileToSearch[locationToCheck].isBlocked || (endTile != null && tileToSearch[locationToCheck] == endTile)))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        //left neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

        if (tileToSearch.ContainsKey(locationToCheck) && (!tileToSearch[locationToCheck].isBlocked || (endTile != null && tileToSearch[locationToCheck] == endTile)))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        return neighbours;
    }

    public List<OverlayTile> GetNeighbourAttackTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles)
    {
        var map = MapManager.Instance.map;

        Dictionary<Vector2Int, OverlayTile> tileToSearch = new Dictionary<Vector2Int, OverlayTile>();

        if (searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                tileToSearch.Add(item.grid2DLocation, item);
            }
        }
        else
        {
            tileToSearch = map;
        }


        List<OverlayTile> neighbours = new List<OverlayTile>();

        //top neighbour
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        //bottom neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        //right neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        //left neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            neighbours.Add(tileToSearch[locationToCheck]);
        }

        return neighbours;
    }
}
