using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//Adding this in for brevity and clarity's sake, this script is meant for the user to use during gameplay.
//Think of the gamepad, people call it a "Controller" because you use it to "control" what's happening on screen.
//Hence this class is called "MouseController"

public class MouseController : MonoBehaviour
{
    public GameObject playerUnitPrefab;
    private PlayerUnitScript pUnit;
    public float speed;
    public GameObject cursor;

    private AStarPathfinder pathFinder;
    private RangefinderMovement rangeFinder;
    private List<HideAndShowScript> path;
    private List<HideAndShowScript> inRangeTiles = new List<HideAndShowScript>();

    // Start is called before the first frame update
    void Start()
    {
        pathFinder = new AStarPathfinder();
        path = new List<HideAndShowScript>();
        rangeFinder = new RangefinderMovement();
        inRangeTiles = new List<HideAndShowScript>();
    }

    // LateUpdate is called at the END of a previous update function call.
    void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();
        
        if (focusedTileHit.HasValue)
        {
            HideAndShowScript overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<HideAndShowScript>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder+1;

            if (Input.GetMouseButtonDown(0))
            {
                //This call essentially means it's getting the component WITHIN the hide&show script
                //To be quite honest, it's more accurate to call that script OverlayTileScript instead
                //I kinda messed up there, really sorry guys.

                //Commenting out the line before because it's creating bugs with the MoveMentRangeFinder and AStarPathfinder scripts
                //overlayTile.GetComponent<HideAndShowScript>().ShowTile();
                if(pUnit == null)
                {
                    pUnit = Instantiate(playerUnitPrefab).GetComponent<PlayerUnitScript>();
                    PositionCharacterOnTile(overlayTile);
                    GetInRangeTiles();
                } else
                {
                    Debug.Log("Checking path value before: " + path);
                    path = pathFinder.FindPath(pUnit.activeTile, overlayTile, inRangeTiles);
                    Debug.Log("Checking path value after: " + path);
                }
                
            }
        }

        if (path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        
        var zIndex = path[0].transform.position.z;
        pUnit.transform.position = Vector2.MoveTowards(pUnit.transform.position, path[0].transform.position, step);
        pUnit.transform.position = new Vector3(pUnit.transform.position.x, pUnit.transform.position.y, zIndex);

        if(Vector2.Distance(pUnit.transform.position, path[0].transform.position) < 0.0001f)
        {
            PositionCharacterOnTile(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GetInRangeTiles();
        }
    }

    //Meant to work with the player movement scripts to get tiles in range
    private void GetInRangeTiles()
    {
        foreach (var item in inRangeTiles) 
        {
            item.HideTile();
        }

        inRangeTiles = rangeFinder.GetTilesInRange(pUnit.activeTile, 3);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {

        //To tell you what's happening here, essentially we're getting the position of the mouse relative to 3D coordinates
        //Although, since we want the position of the mouse in 2D terms, we switch it from Vector3, which is 3D, to Vector2, which is 2d.
        //we also use raycast to actually "select" the tile to focus on it as well.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }

    //This function adjusts the position of the character/unit on the gameplay tile
    private void PositionCharacterOnTile(HideAndShowScript tile)
    {
        pUnit.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z+1);
        pUnit.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder+1;
        pUnit.activeTile = tile;
    }

}
