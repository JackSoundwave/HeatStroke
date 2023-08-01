using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackRangeFinder
{
    // int range defines how far the player can attack
    public List<HideAndShowScript> GetTilesInAttackRange(HideAndShowScript startingTile, int range)
    {
        var inRangeTiles = new List<HideAndShowScript>();

        // Add the starting tile to the list of attackable tiles
        //inRangeTiles.Add(startingTile);

        // Check tiles in the four cardinal directions: up, down, left, right

        //NOTE: I removed the "!tile.isBlocked" boolean because we need a different boolean to dictate whether or not a player can attack them.
        //HideAndShowScript (the tile script) needs to be updated to include a new boolean, "isOccupied", which dictates whether or not there is an entity on top of it.
        //isBlocked, only applies for tiles that have a STRUCTURE on top of them, like a mountain or something similar.
        for (int i = 1; i <= range; i++)
        {
            // Up
            var upTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x, startingTile.gridLocation.y + i);
            if (upTile != null)
            {
                inRangeTiles.Add(upTile);
            }

            // Down
            var downTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x, startingTile.gridLocation.y - i);
            if (downTile != null)
            {
                inRangeTiles.Add(downTile);
            }

            // Left
            var leftTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x - i, startingTile.gridLocation.y);
            if (leftTile != null)
            {
                inRangeTiles.Add(leftTile);
            }

            // Right
            var rightTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x + i, startingTile.gridLocation.y);
            if (rightTile != null)
            {
                inRangeTiles.Add(rightTile);
            }
        }

        return inRangeTiles;
    }

    public HideAndShowScript getAdjacentUpTile(HideAndShowScript startingTile)
    {
        var upTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x, startingTile.gridLocation.y + 1);
        return upTile;
    }

    public HideAndShowScript getAdjacentDownTile(HideAndShowScript startingTile)
    {
        var downTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x, startingTile.gridLocation.y - 1);
        return downTile;
    }

    public HideAndShowScript getAdjacentRightTile(HideAndShowScript startingTile)
    {
        var rightTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x + 1, startingTile.gridLocation.y);
        return rightTile;
    }

    public HideAndShowScript getAdjacentLeftTile(HideAndShowScript startingTile)
    {
        var leftTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x - 1, startingTile.gridLocation.y);
        return leftTile;
    }
}
