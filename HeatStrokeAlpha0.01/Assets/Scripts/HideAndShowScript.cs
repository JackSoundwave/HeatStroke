using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: THIS SCRIPT IS FOR EVERY OVERLAYTILE GAME OBJECT
public class HideAndShowScript : MonoBehaviour
{
    public int G;
    public int H;

    public int F { get { return G + H; } }


    public bool isBlocked;
    public bool isDeployTile;
    public HideAndShowScript Previous;

    //this Entity field dictates WHAT is currently on top of the tile. It's a gameObject as every entity would be a gameObject.
    public GameObject entity;

    //Grid location, in 3d space, so that the Z axis can act as a sorting layer
    public Vector3Int gridLocation;

    //This version is just the 2D instance of the gridLocation variable, used for functions than can only pass a vector2int variable into them.
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

// This function SHOWS the tile
    public void ShowTile()
    {
        /* just a heads up the new Color() is sorted as RGBA,
        new Color (R , G , B , A);*/

        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void DyeTileRed()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void DyeTileBlue()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void unBlockSelf()
    {
        isBlocked = false;
    }

    public void DyeTileYellow()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void DyeTileGreen()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
