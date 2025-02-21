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

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder;
                        overlayTile.gridLocation = tileLocation;
                        map.Add(tilekey, overlayTile);
                    }
                }
            }
 //       }
    }

    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbours = new List<OverlayTile>();

        //top neighbour
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //bottom neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //right neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        //left neighbour
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }
}
