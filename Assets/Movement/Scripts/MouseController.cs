using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ArrowTranslator;

public class MouseController : MonoBehaviour
{
    private static MouseController _instance;
    public static MouseController Instance { get { return _instance; } }

    public GameObject characterPrefab;
    private CharacterInfo character;
    public float speed;

    private Pathfinder pathfinder;
    private RangeFinder rangeFinder;
    private ArrowTranslator arrowTranslator;
    private List<OverlayTile> path = new List<OverlayTile>();
    public List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> inAttackRangeTiles = new List<OverlayTile>();

    public bool isMoving = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathfinder = new Pathfinder();
        rangeFinder = new RangeFinder();
        arrowTranslator = new ArrowTranslator();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();

        if(focusedTileHit.HasValue)
        {
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();

            if (overlayTile == null)
                return;

            transform.position = new Vector3(overlayTile.transform.position.x, overlayTile.transform.position.y, overlayTile.transform.position.z-1);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (inRangeTiles.Contains(overlayTile) && !isMoving && character.canMove)
            {
                path = pathfinder.FindPath(character.activeTile, overlayTile, inRangeTiles);

                foreach(var item in inRangeTiles)
                {
                    item.SetArrowSprite(ArrowDirection.None);
                }

                GetInAttackRangeTiles(overlayTile);

                for (int i = 0; i < path.Count; i++)
                {
                    var previousTile = i > 0 ? path[i-1] : character.activeTile;
                    var futureTile = i < path.Count - 1 ? path[i+1] : null;

                    var arrowDir = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                    path[i].SetArrowSprite(arrowDir);
                }

                
            }

            if (Input.GetMouseButtonDown(0) && inRangeTiles.Contains(overlayTile) && character.canMove)
            {
                /*if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    PositionCharacterOnTile(overlayTile);
                    GetInRangeTiles();
                }
                else
                {
                    if(inRangeTiles.Contains(overlayTile))
                        isMoving = true;
                }*/

                isMoving = true;
                character.canAct = true;
            }

            if (Input.GetMouseButtonDown(0) && inAttackRangeTiles.Contains(overlayTile))
            {
                if (overlayTile.collisionGO == null)
                    return;

                //Debug.Log(character.name+" attacks "+overlayTile.collisionGO.name);
                if (overlayTile.collisionGO.GetComponent<CharacterInfo>() != null)
                {
                    character.PerformAttack(0, overlayTile.collisionGO.GetComponent<CharacterInfo>());
                    character.canAct = false;

                }
                else
                {
                    Debug.Log("Can't hit");
                }
            }
        }

        if(path.Count > 0 && isMoving)
        {
            MoveAlongPath();
            FindAnyObjectByType<Camera>().transform.position = new Vector3(character.transform.position.x, character.transform.position.y, -10);
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

        if(path.Count == 0)
        {
            //GetInRangeTiles();
            HideRangeTiles();
            isMoving = false;
            character.canMove = false;
        }     
    }

    private void PositionCharacterOnTile(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
    }

    public void PositionCharacterOnTile(CharacterInfo chara, OverlayTile tile)
    {
        chara.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        chara.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        chara.activeTile = tile;
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if(hits.Length > 0)
        {
            return hits[hits.Length-1];
            //return hits.OrderByDescending(i => i.collider.transform.position).First();
        }

        return null;
    }

    public void GetInRangeTiles()
    {
        HideRangeTiles();

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, character.range);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile(0);
        }
    }

    public void GetInAttackRangeTiles(OverlayTile tile)
    {
        HideAttackRangeTiles();

        int range = 0;
        foreach (Attacks attack in character.attacks)
        {
            if(attack.attackName == character.activeAtk)
            {
                range = attack.range;
                break;
            }
        }

        inAttackRangeTiles = rangeFinder.GetTilesInAttackRange(tile, range);

        foreach (var item in inAttackRangeTiles)
        {
            item.ShowTile(1);
        }
    }

    public void HideRangeTiles()
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
            if(character.canAct)
            {
                foreach (var item2 in inAttackRangeTiles)
                {
                    if (item2 == item)
                        item.ShowTile(1);
                }
            }
        }
    }

    public void HideAttackRangeTiles()
    {
        foreach (var item in inAttackRangeTiles)
        {
            item.HideTile();
            if (character.canMove)
            {
                foreach (var item2 in inRangeTiles)
                {
                    if (item2 == item)
                        item.ShowTile(0);
                }
            }
        }
    }

    public void SetCharacter(CharacterInfo chara)
    {
        character = chara;
    }
}
