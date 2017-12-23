using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public static int w = 10;
    public static int h = 13;
    public static Element[,] elements = new Element[w,h];

    public static void UncoverMines()
    {
        foreach (Element elem in elements)
            if (elem.mine)
                elem.LoadTexture(0);
    }
    //Is mine at coords
    public static bool MineAt(int x, int y)
    {
        //Coordinates in range: check for mine
        if(x>=0 && y >= 0 && x < w && y < h)
        {
            return elements[x, y].mine;
        }
        return false;
    }

    public static int AdjacentMines(int x, int y)
    {
        int count = 0;

        //count adjacent mines
        if (MineAt(x, y + 1)) ++count; // top
        if (MineAt(x + 1, y + 1)) ++count; // top-right
        if (MineAt(x + 1, y)) ++count; // right
        if (MineAt(x + 1, y - 1)) ++count; // bottom-right
        if (MineAt(x, y - 1)) ++count; // bottom
        if (MineAt(x - 1, y - 1)) ++count; // bottom-left
        if (MineAt(x - 1, y)) ++count; // left
        if (MineAt(x - 1, y + 1)) ++count; // top-left

        return count;
    }

    public static void FFuncover(int x, int y, bool[,] visited)
    {
        // Coordinates in Range?
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            //visited already
            if (visited[x, y])
                return;

            // uncover element
            elements[x, y].LoadTexture(AdjacentMines(x, y));

            // close to a mine? then no more work needed here
            if (AdjacentMines(x, y) > 0)
                return;

            //set visited flag
            visited[x, y] = true;

            //recursion
            FFuncover(x - 1, y, visited);
            FFuncover(x + 1, y, visited);
            FFuncover(x, y - 1, visited);
            FFuncover(x, y + 1, visited);
        }
    }

    public static bool IsFinished()
    {
        // Try to find a covered element that is no mine
        foreach (Element elem in elements)
            if (elem.IsCovered() && !elem.mine)
                return false;
        // There are none => all are mines => game won.
        return true;
    }
}
