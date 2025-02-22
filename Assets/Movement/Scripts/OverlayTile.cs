using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    public int G;
    public int H;

    public int F { get { return G + H; } }

    public bool isBlocked;

    public OverlayTile previous;

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }


    // Update is called once per frame
    /*    void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HideTile();
            }
        }*/

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new  Color(1, 1, 1, 1);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isBlocked = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isBlocked = false;
    }
}
