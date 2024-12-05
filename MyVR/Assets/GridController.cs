using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    //The quad used to display the cells
    public GameObject cellQuadObj;
    public GameObject gameQuadObj;

    //The size of one cell
    public float cellSize = 0.2f;
    //How many cells do we have in one row?
    //public int gridSize = 20;


    //Game grid / Level
    int[,] grid1 =
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
        {0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
        {0, 0, 1, 1, 1, 1, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

    //To make it easier to access the script from other scripts
    public static GridController current;


    public int[,] grid;
    void Start()
    {
        grid = grid1;
        Console.WriteLine("Start");
        System.Console.WriteLine("Start");
        current = this;

        Vector3 gridCenter = transform.position;

        //Display the grid cells with quads
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int z = 0; z < grid.GetLength(1); z++)
            {

                Vector3 centerPos = new Vector3(x * cellSize, 0f, z * cellSize);

                if (grid[x, z] == 0)
                {
                    GameObject newCellQuad = Instantiate(cellQuadObj, centerPos + gridCenter, Quaternion.identity, transform);
                    newCellQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    newCellQuad.SetActive(true);
                }
                else
                {
                    GameObject newCellQuad = Instantiate(gameQuadObj, centerPos + gridCenter, Quaternion.identity, transform);
                    newCellQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    newCellQuad.SetActive(true);
                }

                
            }
        }

        Renderer originalRenderer = cellQuadObj.GetComponent<Renderer>();
        originalRenderer.enabled = false;
        Renderer gameRenderer = gameQuadObj.GetComponent<Renderer>();
        gameRenderer.enabled = false;
    }



    /*
    //Is a world position within the grid?
    public bool IsWorldPosInGrid(Vector3 worldPos)
    {
        bool isWithin = false;

        Vector3 relativePos = worldPos - transform.position;

        int gridX = TranslateFromWorldToGrid(relativePos.x);
        int gridZ = TranslateFromWorldToGrid(relativePos.z);

        if (gridX >= 0 && gridZ >= 0 && gridX < gridSize && gridZ < gridSize)
        {
            isWithin = true;
        }

        return isWithin;
    }

    //Translate from world position to grid position
    private int TranslateFromWorldToGrid(float pos)
    {
        int gridPos = Mathf.FloorToInt(pos / cellSize);
        Console.WriteLine(Mathf.FloorToInt(pos / 1f));
        Console.WriteLine(Mathf.FloorToInt(pos / 0.4f));
        System.Console.WriteLine(Mathf.FloorToInt(pos / 1f));
        System.Console.WriteLine(Mathf.FloorToInt(pos / 0.4f));

        return gridPos;
    }*/
}