using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyCalculateTargetState : EnemyAIBaseScript
{
    private Dictionary<HideAndShowScript, float> tileScores = new Dictionary<HideAndShowScript, float>();
    private List<KeyValuePair<HideAndShowScript, float>> sortedDictionary = new List<KeyValuePair<HideAndShowScript, float>>();

    private HideAndShowScript bestTile;

    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Enemy is calculating best move :o");
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        
    }

    public void calculateMovementTileScores(EnemyAIStateManager enemy)
    {
        //this script should essentially calculate all tiles currently movable by the enemy.
        /*
         * Calculation is based on 3 factors:
         * 1. Distance to target (in tiles) (defenseStructure has a higher score value than players) (Adjacent tiles = 3points, +2 if the tile.entity is a defenseStructure)
         * 2. If no targets are adjacent within movement range (I.E, all score values are less than or equal to 0), just move as close as possible to a random target. (it can be either a defenseStructure or a playerUnit).
         * 3. If tile is currently about to be attacked by enemy, SET tileScore in the tileScore/tile pair to -5. (need to implement this later)
         * 4. If a target can be found within the immediate movement range, move towards and attempt to attack that target instead.
         */

        //clear previous tileScores to prevent duplicate entries
        tileScores.Clear();
        sortedDictionary.Clear();

        enemy.movement.GetInRangeTiles();
        foreach(HideAndShowScript tile in enemy.movement.inRangeTiles)
        {
            tileScores.Add(tile, 0);
        }

        foreach(HideAndShowScript tile in enemy.movement.inRangeTiles)
        {
            List<HideAndShowScript> tileAttack = new List<HideAndShowScript>();
            tileAttack = enemy.attack.attackRangeFinder.GetTilesInAttackRange(tile, enemy.thisUnit.attackRange);

            float tempScore = 0;

            foreach(HideAndShowScript atkTile in tileAttack)
            {
                if (atkTile.entity != null)
                {
                    PlayerUnitScript tempPlayer = atkTile.entity.GetComponent<PlayerUnitScript>();
                    DefenceStructure tempStruct = atkTile.entity.GetComponent<DefenceStructure>();

                    if (tempPlayer != null)
                    {
                        tempScore = tempScore + enemy.playerBias;
                    }
                    else if (tempStruct != null)
                    {
                        tempScore = tempScore + enemy.structureBias;
                    }
                    else
                    {
                        Debug.Log("Unknown entity, what the hell is that?");
                    }
                }
            }
            tileScores[tile] = tempScore;
        }
    }

    public void calculateAttackTileScores(EnemyAIStateManager enemy)
    {
        enemy.attack.getAttackRange();
    }

    public void filterZeroTiles()
    {
        
    }

    public void sortTileScores() 
    {
        var sortedList = tileScores.OrderByDescending(x => x.Value).ToList();
        sortedDictionary = sortedList;
    }

    public List<KeyValuePair<HideAndShowScript, float>> getFirstFive()
    {
        sortedDictionary = sortedDictionary.Take(5).ToList();
        return sortedDictionary;
    }

    public HideAndShowScript selectTile()
    {
        /*
         * When attempting to choose a tile move from the sorted list, what should be passed into here is both the target for the movement script
         * And also the targetTile to move to (which should be an adjsacent tile).
         * Based on some sort of difficulty factor, we also want to sometimes choose the "worse" option. Just to make it easier for players down the line.
         * But let's be real it's too late into development to add that. No seriously. it really is.
         * For now we'll just make it pick randomly from the top 5 options, with a bias towards the 2nd and 3rd option.
         */

        int chosenIndex = Random.Range(0, 3);

        HideAndShowScript randomTile = sortedDictionary[chosenIndex].Key;
        return randomTile;
    }
}
