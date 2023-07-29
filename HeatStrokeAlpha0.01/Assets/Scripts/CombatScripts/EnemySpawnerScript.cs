using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    /*
     * basic idea for this script, needs to be updated in the future to include random enemies, and instead to spawn a prefab called "enemyAboutToSpawn".
     * 
     * enemyAboutToSpawn is a prefab that selects it's own activeTile, sets that tile to "enemyAboutToSpawn = true", which prevents the spawner from creating a new prefab
     * on that specific tile.
     * 
     * When the enemyAboutToSpawn prefab dies, it spawns an enemyUnit from a random list of pre-determined prefabs. Depending on things like difficulty, current level, etc.
     * But for now
    */

    
    public GameObject enemyUnitPrefab;


    public bool showSpawnTiles, AddDefaultSpawnTiles;


    private GameObject eu_GO;

    public List<int> tileNumbersToMark;

    [HideInInspector]
    public List<int> defaultZone = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

    [SerializeField]
    private int maxEnemies;

    
    [Range(0f, 5f)]
    public float spawnDelay;

    private List<Vector2Int> unblockedTiles;
    private MapManager mapManager;
    private List<KeyValuePair<Vector2Int, HideAndShowScript>> overlayTiles;

    private void Start()
    {
        if (AddDefaultSpawnTiles)
        {
            tileNumbersToMark.AddRange(defaultZone);
        }
        mapManager = FindObjectOfType<MapManager>();
        Debug.Log("MapManager object: " + mapManager);
        GameEventSystem.current.onGridGenerated += SpawnEnemyOnRandomTile;
        GameEventSystem.current.onEnemyTurnEnd += OnEnemyTurnEnd;

        if(showSpawnTiles == true)
        {
            GameEventSystem.current.onGridGenerated += ShowMarkedTiles;
        }
    }

    /*This updated version of "spawn enemy on random tile" requires additional factors, 
     * like considering how many enemies are currently on the grid for examps, 
     * and whether or not 3 enemies are already about to spawn.
    */
    private void SpawnEnemyOnRandomTile()
    {
        Debug.Log("Executing SpawnEnemyOnRandomTile");
        overlayTiles = mapManager.GetOverLayTiles();

        List<int> filteredList = FilterOutBlockedTiles(tileNumbersToMark);

        ShuffleList(filteredList);
        Debug.Log(GameEventSystem.current.enemyUnits.Count);
        int randomNo = Random.Range(0, 2);
        for (int i = 0; i <= randomNo; i++)
        {
            filteredList = FilterOutBlockedTiles(tileNumbersToMark); //overwrite the filteredlist once more
            ShuffleList(filteredList); //shuffle list again

            foreach (int indexToMark in filteredList)
            {
                if (indexToMark >= 0 && indexToMark < overlayTiles.Count)
                {
                    if (GameEventSystem.current.enemyUnits.Count < maxEnemies)
                    {
                        KeyValuePair<Vector2Int, HideAndShowScript> tileEntry = overlayTiles[indexToMark];
                        HideAndShowScript tile = tileEntry.Value;
                        SpawnEnemy(tile);
                        
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Too many enemies");
                        break;
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid index to mark: " + indexToMark);
                }
            }
        }
    }

    private void OnEnemyTurnEnd()
    {
        StartCoroutine(SpawnEnemiesWithDelay());
    }
    private IEnumerator SpawnEnemiesWithDelay()
    {
        //Debug.Log("Executing SpawnEnemyOnRandomTile");
        overlayTiles = mapManager.GetOverLayTiles();

        List<int> filteredList = FilterOutBlockedTiles(tileNumbersToMark);

        ShuffleList(filteredList);
        Debug.Log(GameEventSystem.current.enemyUnits.Count);
        int randomNo = Random.Range(0, 2);

        for (int i = 0; i <= randomNo; i++)
        {
            filteredList = FilterOutBlockedTiles(tileNumbersToMark); //overwrite the filteredlist once more
            ShuffleList(filteredList); //shuffle list again

            foreach (int indexToMark in filteredList)
            {
                if (indexToMark >= 0 && indexToMark < overlayTiles.Count)
                {
                    if (GameEventSystem.current.enemyUnits.Count < maxEnemies)
                    {
                        KeyValuePair<Vector2Int, HideAndShowScript> tileEntry = overlayTiles[indexToMark];
                        HideAndShowScript tile = tileEntry.Value;
                        SpawnEnemy(tile);

                        yield return new WaitForSeconds(spawnDelay); //Introduce a delay before the next iteration
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Too many enemies");
                        yield return new WaitForSeconds(spawnDelay); //Introduce a delay before the next iteration
                        break;
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid index to mark: " + indexToMark);
                }
            }
        }
    }

    private void ShowMarkedTiles()
    {
        overlayTiles = mapManager.GetOverLayTiles();

        foreach (int indexToMark in tileNumbersToMark)
        {
            if (indexToMark >= 0 && indexToMark < overlayTiles.Count)
            {
                KeyValuePair<Vector2Int, HideAndShowScript> tileEntry = overlayTiles[indexToMark];
                HideAndShowScript tile = tileEntry.Value;

                tile.DyeTileGreen();
            }
            else
            {
                Debug.LogWarning("Invalid index to mark: " + indexToMark);
            }
        }
    }

    private void PositionEnemyOnTile(HideAndShowScript tile)
    {
        eu_GO.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z + 1);
        eu_GO.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 2;
        tile.entity = eu_GO;
    }

    /* Randomization function I found online, I'm gonna keep it real, I literally have no clue how it works.
     * It's apparently called the "Fisher-yates" shuffle
     * You can read about it on wikipedia here
     * https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
     * and I got the implementation from here https://stackoverflow.com/questions/273313/randomize-a-listt
     * Pretty cool
     * Also instead of using "Rng.next" which, I'm not sure what the hell is that, instead we're using Random.Range, derived from Unity's built in classes.
     * I hope unreal has something like this TT
     */
    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private List<int> FilterOutBlockedTiles(List<int> zoneToMark)
    {
        List<int> filteredZone = new List<int>();
        overlayTiles = mapManager.GetOverLayTiles();

        foreach (int indexToMark in zoneToMark)
        {
            if (indexToMark >= 0 && indexToMark < overlayTiles.Count)
            {
                KeyValuePair<Vector2Int, HideAndShowScript> tileEntry = overlayTiles[indexToMark];
                HideAndShowScript tile = tileEntry.Value;

                if (!tile.isBlocked)
                {
                    filteredZone.Add(indexToMark); //Adding integer of unblocked tiles into the list
                }
            }
            else
            {
                Debug.LogWarning("Default Zone could not be resolved");
            }
        }

        return filteredZone;
    }

    private IEnumerator SpawnEnemyWithDelay(HideAndShowScript tileToSpawn)
    {
        Debug.Log("Enemyspawned");   
        if (GameEventSystem.current.enemyUnits.Count < maxEnemies)
        {
            yield return new WaitForSeconds(spawnDelay);
            eu_GO = Instantiate(enemyUnitPrefab);
            eu_GO.GetComponent<EnemyUnitScript>().activeTile = tileToSpawn;
            PositionEnemyOnTile(tileToSpawn);
            tileToSpawn.isBlocked = true;
            eu_GO = null;
            yield return new WaitForSeconds(spawnDelay);
        }
        else
        {
            Debug.Log("No Enemies spawned");
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy(HideAndShowScript tileToSpawn)
    {
        if(GameEventSystem.current.enemyUnits.Count < maxEnemies)
        {
            eu_GO = Instantiate(enemyUnitPrefab);
            eu_GO.GetComponent<EnemyUnitScript>().activeTile = tileToSpawn;
            PositionEnemyOnTile(tileToSpawn);
            tileToSpawn.isBlocked = true;
            eu_GO = null;
        }
        else
        {
            Debug.Log("No Enemies spawned");
        }
    }
}
