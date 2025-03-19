using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static ArrowTranslator;

public class OverlayTile : MonoBehaviour
{
    public int G;
    public int H;

    public int F { get { return G + H; } }

    public bool isBlocked;

    public OverlayTile previous;

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public List<Sprite> arrows;

    public GameObject collisionGO;

    public void ShowTile(int color)
    {
        if(color == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        SetArrowSprite(ArrowDirection.None);
    }

    public void SetArrowSprite(ArrowDirection d)
    {
        var arrow = GetComponentsInChildren<SpriteRenderer>()[1];
        if(d == ArrowDirection.None)
        {
            arrow.color = new Color(1, 1, 1, 0);
        }
        else
        {
            arrow.color = new Color(1, 1, 1, 1);
            arrow.sprite = arrows[(int)d];
            arrow.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isBlocked = true;
        collisionGO = collision.gameObject;
        //collision.transform.position = transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isBlocked = false;
        collisionGO = null;
    }
}
