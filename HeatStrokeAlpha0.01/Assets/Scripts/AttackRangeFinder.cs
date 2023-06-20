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
        inRangeTiles.Add(startingTile);

        // Check tiles in the four cardinal directions: up, down, left, right
        for (int i = 1; i <= range; i++)
        {
            // Up
            var upTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x, startingTile.gridLocation.y + i);
            if (upTile != null && !upTile.isBlocked)
            {
                inRangeTiles.Add(upTile);
            }

            // Down
            var downTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x, startingTile.gridLocation.y - i);
            if (downTile != null && !downTile.isBlocked)
            {
                inRangeTiles.Add(downTile);
            }

            // Left
            var leftTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x - i, startingTile.gridLocation.y);
            if (leftTile != null && !leftTile.isBlocked)
            {
                inRangeTiles.Add(leftTile);
            }

            // Right
            var rightTile = MapManager.instance2.getTileAtPosition(startingTile.gridLocation.x + i, startingTile.gridLocation.y);
            if (rightTile != null && !rightTile.isBlocked)
            {
                inRangeTiles.Add(rightTile);
            }
        }

        return inRangeTiles;
    }
}
