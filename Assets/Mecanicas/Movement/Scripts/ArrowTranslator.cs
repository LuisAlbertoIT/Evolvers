using UnityEngine;

public class ArrowTranslator
{
    public enum ArrowDirection
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
        Up = 3,
        Down = 4,
        Left = 5,
        Right = 6,
        UpRight = 7,
        UpLeft = 8,
        DownRight = 9,
        DownLeft = 10,
    }

    public ArrowDirection TranslateDirection(OverlayTile previousTile, OverlayTile currentTile, OverlayTile futureTile)
    {
        bool isFinal = futureTile == null;

        Vector2Int pastDirection = previousTile != null ? currentTile.grid2DLocation - previousTile.grid2DLocation : new Vector2Int(0,0);
        Vector2Int futureDirection = futureTile != null ? futureTile.grid2DLocation - currentTile.grid2DLocation : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if(direction == new Vector2Int(0,1) && !isFinal)
        {
            return ArrowDirection.Vertical;
        }

        if (direction == new Vector2Int(0, -1) && !isFinal)
        {
            return ArrowDirection.Vertical;
        }

        if (direction == new Vector2Int(1,0) && !isFinal)
        {
            return ArrowDirection.Horizontal;
        }

        if (direction == new Vector2Int(-1, 0) && !isFinal)
        {
            return ArrowDirection.Horizontal;
        }

        if (direction == new Vector2Int(0, 1) && isFinal)
        {
            return ArrowDirection.Up;
        }

        if (direction == new Vector2Int(0, -1) && isFinal)
        {
            return ArrowDirection.Down;
        }

        if (direction == new Vector2Int(1, 0) && isFinal)
        {
            return ArrowDirection.Right;
        }

        if (direction == new Vector2Int(-1, 0) && isFinal)
        {
            return ArrowDirection.Left;
        }

        if(direction == new Vector2Int(1,1))
        {
            if(pastDirection.y < futureDirection.y)
                return ArrowDirection.DownLeft;
            else
                return ArrowDirection.UpRight;
        }

        if (direction == new Vector2Int(-1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.DownRight;
            else
                return ArrowDirection.UpLeft;
        }

        if (direction == new Vector2Int(1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.UpLeft;
            else
                return ArrowDirection.DownRight;
        }

        if (direction == new Vector2Int(-1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.UpRight;
            else
                return ArrowDirection.DownLeft;
        }

        return ArrowDirection.None;
    }
}
