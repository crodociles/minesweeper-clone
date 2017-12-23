using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour {
    
    //Is it a mine
    public bool mine;

    //Different textures
    public Sprite[] emptyTextures;
    public Sprite mineTexture;

	// Use this for initialization
	void Start () {
        //Randomly decide if mine
        mine = Random.value < 0.15;

        // Register in Grid
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        Grid.elements[x, y] = this;
    }

    //Load another texture
    public void LoadTexture(int adjacentCount)
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
        }
    }

    // Is it still covered?
    public bool IsCovered()
    {
        return GetComponent<SpriteRenderer>().sprite.texture.name == "default";
    }

    private void OnMouseUpAsButton()
    {
        if (mine)
        {
            //uncover all mines
            Grid.UncoverMines();

            //gameover
            print("You lose");
        }
        else
        {
            //Show number of adjacent mines
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            LoadTexture(Grid.AdjacentMines(x, y));

            //uncover area without mines
            Grid.FFuncover(x, y, new bool[Grid.w, Grid.h]);

            //Find out if game won
            if (Grid.IsFinished())
                print("you win");
        }
    }
}
