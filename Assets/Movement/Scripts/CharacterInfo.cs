using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public int range = 3;
    public int attackRange = 1;
    public bool canMove = true;
    public bool canAct = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        activeTile = collision.GetComponent<OverlayTile>();
        activeTile.isBlocked = true;
        //collision.transform.position = transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activeTile = collision.GetComponent<OverlayTile>();
        activeTile.isBlocked = false;
    }
}
