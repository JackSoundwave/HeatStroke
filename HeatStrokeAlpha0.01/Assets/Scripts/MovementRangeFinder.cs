using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RangefinderMovement
{
    //int range defines how far the player can move
    public List<HideAndShowScript> GetTilesInRange(HideAndShowScript startingTile, int range)
    {
        var inRangeTiles = new List<HideAndShowScript>();
        int stepCount = 0;

        inRangeTiles.Add(startingTile);

        var tileForPreviousStep = new List<HideAndShowScript>();
        tileForPreviousStep.Add(startingTile);

        while(stepCount < range)
        {
            var surroundingTile = new List<HideAndShowScript>();

            foreach(var item in tileForPreviousStep)
            {
                surroundingTile.AddRange(MapManager.instance2.getNeighbourTiles(item, new List<HideAndShowScript>()));
            }

            inRangeTiles.AddRange(surroundingTile);
            tileForPreviousStep = surroundingTile.Distinct().ToList();
            stepCount++;
        }

        return inRangeTiles.Distinct().ToList();
    }

}
