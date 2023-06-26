using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinder
{

    //This function is used to get the block distance so that it creates a movable square.
    private int GetManhattanDistance(HideAndShowScript start, HideAndShowScript neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    //Finds the valid path the player can traverse
    public List<HideAndShowScript> FindPath(HideAndShowScript start, HideAndShowScript end, List<HideAndShowScript> searchableTiles)
    {
        //Debug.Log("Executing FindPath()");
        List<HideAndShowScript> openList = new List<HideAndShowScript>();
        List<HideAndShowScript> closedList = new List<HideAndShowScript>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            HideAndShowScript currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            //finalise path
            if(currentOverlayTile == end)
            {
                //Debug.Log("Checking end: " + end);
                //Debug.Log("Checking start: " + start);
                //Debug.Log("Checking currentOverlayTile:" + currentOverlayTile);
                return GetFinishedList(start, end);
            }


            var neighbourTiles = MapManager.instance2.getNeighbourTiles(currentOverlayTile, searchableTiles);

            foreach (var  tile in neighbourTiles)
            {
                //The number "1" in this if statement, is essentially the jump height of the tile.
                //Although we won't be using any of that in this game because, duh. It's ground based.
                if (tile.isBlocked || closedList.Contains(tile) || Mathf.Abs (currentOverlayTile.gridLocation.z - tile.gridLocation.z) > 1)
                {
                    continue;
                }

                tile.G = GetManhattanDistance(start, tile);
                tile.H = GetManhattanDistance(end, tile);

                tile.Previous = currentOverlayTile;

                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<HideAndShowScript>();
    }

    private List<HideAndShowScript> GetFinishedList(HideAndShowScript start, HideAndShowScript end)
    {
        Debug.Log("Executing GetFinishedList()");
        List<HideAndShowScript> finishedList = new List<HideAndShowScript>();
        
        HideAndShowScript currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();
        return finishedList;
    }

}
