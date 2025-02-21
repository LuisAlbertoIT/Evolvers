using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject characterPrefab;
    private CharacterInfo character;
    public float speed;

    private Pathfinder pathfinder;
    private List<OverlayTile> path = new List<OverlayTile>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathfinder = new Pathfinder();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();

        if(focusedTileHit.HasValue)
        {
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = new Vector3(overlayTile.transform.position.x, overlayTile.transform.position.y, overlayTile.transform.position.z-1);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                //overlayTile.GetComponent<OverlayTile>().ShowTile();
                overlayTile.ShowTile();

                if(character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnTile(overlayTile);
                }
                else
                {
                    path = pathfinder.FindPath(character.activeTile, overlayTile);
                }
            }
        }

        if(path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if(Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position).First();
        }

        return null;
    }
}
