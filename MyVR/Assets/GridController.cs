using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    //The quad used to display the cells
    public GameObject cellQuadObj;

    //The size of one cell
    public float cellSize = 0.2f;
    //How many cells do we have in one row?
    public int gridSize = 20;

    //To make it easier to access the script from other scripts
    public static GridController current;

    void Start()
    {
        Console.WriteLine("Start");
        System.Console.WriteLine("Start");
        current = this;

        Vector3 gridCenter = transform.position;

        //Display the grid cells with quads
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                //The center position of the cell
                //Vector3 centerPos = new Vector3(x + cellSize / 2f, 0f, z + cellSize / 2f);
                //Vector3 centerPos = new Vector3(x + cellSize / 2f, 0f, z + cellSize / 2f) + transform.position;
                //Vector3 centerPos = new Vector3(x * cellSize, 0f, z * cellSize) + transform.position;
                Vector3 centerPos = new Vector3(x * cellSize, 0f, z * cellSize);

                //GameObject newCellQuad = Instantiate(cellQuadObj, centerPos, Quaternion.identity, transform);
                // GameObject newCellQuad = Instantiate(cellQuadObj, centerPos, Quaternion.Euler(90f, 0f, 0f), transform);
                GameObject newCellQuad = Instantiate(cellQuadObj, centerPos + gridCenter, Quaternion.identity, transform);

                newCellQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                newCellQuad.SetActive(true);
            }
        }
    }

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
    }
}