using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchableTiles)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end )
            {
                //finalize our tile
                return GetFinishedList(start, end);
            }

            var neighbourTiles = MapManager.Instance.GetNeighbourTiles(currentOverlayTile, searchableTiles, end);

            foreach ( var neighbour in neighbourTiles )
            {
                if( neighbour.isBlocked && neighbour!=end)
                {
                    continue;
                }
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                neighbour.G = GetManhattenDistance(start, neighbour);
                neighbour.H = GetManhattenDistance(end, neighbour);

                neighbour.previous = currentOverlayTile;

                if(!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();

        OverlayTile currentTile = end;

        while (currentTile != start)
        { 
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();
        return finishedList;
    }

    private int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
    {
        return Math.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Math.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }
}
