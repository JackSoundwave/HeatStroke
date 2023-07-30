using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCalculateTargetState : EnemyAIBaseScript
{
    
    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is calculating best move :o");
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        
    }

    public void calculateTileScores()
    {
        //this script should essentially calculate all tiles currently movable by the enemy.
        /*
         * Calculation is based on 3 factors:
         * 1. Distance to target (in tiles) (defenseStructure has a higher score value than players) (Adjacent tiles = 3points, +2 if the tile.entity is a defenseStructure)
         * 2. If no targets are adjacent within movement range (I.E, all score values are less than or equal to 0), just move as close as possible to a random target. (it can be either a defenseStructure or a playerUnit).
         * 3. If tile is currently about to be attacked by enemy, SET tileScore in the tileScore/tile pair to -5.
         * 4. If a target can be found within the immediate movement range, move towards and attempt to attack that target instead.
         */
    }

    public void sortTileScores() 
    { 

    }

    public void selectOptimalTile()
    {
        /*
         * When attempting to choose a tile move from the sorted list, what should be passed into here is both the target for the movement script
         * And also the targetTile to move to (which should be an adjsacent tile).
         * Based on some sort of difficulty factor, we also want to sometimes choose the "worse" option. Just to make it easier for players down the line.
         * But let's be real it's too late into development to add that. No seriously. it really is.
         * For now we'll just make it pick randomly from the top 3 options, with a bias towards the 2nd option.
         */
    }
}
