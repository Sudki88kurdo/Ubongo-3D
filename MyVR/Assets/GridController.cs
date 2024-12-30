using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GridController : MonoBehaviour
{
    //Größe eines Grids
    public float cellSize = 0.2f;

    //Quads um Grid anzuzeigen
    public GameObject cellQuadObj;
    public GameObject gameQuadObj;

    // Erweiterung: Bei jedem Level werden Bausteine erzeugt
    public GameObject blockZ;
    public GameObject blockT;
    public GameObject blockPlus;
    public GameObject blockL;
    public GameObject blockI;
    public GameObject blockMinus;
    public GameObject blockRhomb;

    private Dictionary<int, List<GameObject>> levelBlocks = new Dictionary<int, List<GameObject>>();
    private List<GameObject> activeBlocks = new List<GameObject>();


    private Dictionary<int, int[,]> levelGrids = new Dictionary<int, int[,]>();

    private int level = 1;



    // Erweiterung: Automatische Erstellung von Grids anhand der Flächen der Bausteine
    //Game grid / Level
    int[,] grid1 =
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };


    int[,] grid2 =
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };


    int[,] grid3 =
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

    int[,] testGrid =
    {
        {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };


    //To make it easier to access the script from other scripts
    public static GridController current;

    private List<GameObject> cellQuads; // Dynamische Liste
    private List<GameObject> gameQuads; // Dynamische Liste

    private List<GameObject> blocks;

    private GameObject generalTarget;

    private float previousY = -1;
    private float noMovementThreshold = 0.01f; 
    private float stationaryTime = 0f;
    private float stationaryTimeLimit = 0.5f;


    void Start()
    {
        cellQuads = new List<GameObject>();
        gameQuads = new List<GameObject>();
        current = this;

        blocks = new List<GameObject>();
        blocks.Add(blockZ);
        blocks.Add(blockT);
        blocks.Add(blockPlus);
        blocks.Add(blockL);
        blocks.Add(blockI);
        blocks.Add(blockMinus);
        blocks.Add(blockRhomb);


        levelBlocks[1] = new List<GameObject> { blockZ, blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[2] = new List<GameObject> { blockZ, blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[3] = new List<GameObject> { blockZ, blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[4] = new List<GameObject> { blockMinus, blockRhomb };

        levelGrids[1] = grid1;
        levelGrids[2] = grid2;
        levelGrids[3] = grid3;
        levelGrids[4] = testGrid;


        LoadLevel(1);

        TestSolution(level);

    }

    void Update()
    {
        TestMovement();
        TestSolution(level);
    }

    private void TestMovement()
    {
        if (generalTarget == null) return;

        if(previousY < 0)
        {
            previousY = generalTarget.transform.position.y;
            return;
        }
        float deltaY = Mathf.Abs(generalTarget.transform.position.y - previousY);
        if (deltaY < noMovementThreshold)
        {
            stationaryTime += Time.deltaTime;

            // Wenn Baustein nicht mehr fällt
            if (stationaryTime >= stationaryTimeLimit)
            {
                Debug.Log($"{generalTarget} y: {generalTarget.transform.position.y}");
                stationaryTime = 0;
                generalTarget = null;
                previousY = -1;
                
                return;
            } 
        }
        else
        {
            stationaryTime = 0;
                
        }
        previousY = generalTarget.transform.position.y;
    }

    private void TestSolution(int level)
    {
        int[,] grid = levelGrids[level];
        //ToDo Teste ob die Lösung des Spielers richtig ist
        int[,] solutionGrid = new int[grid.GetLength(0), grid.GetLength(1)];

        /*
        Debug.Log($"grid {grid.ToString()} ");
        MatrixInConsole(grid);
        Debug.Log($"sol {solutionGrid.ToString()} ");
        MatrixInConsole(solutionGrid);

        Debug.Log($"cell {cellQuadObj.transform.position}");

        Debug.Log($"game {gameQuadObj.transform.position} ");

        */

        foreach(GameObject block in activeBlocks)
        {

            foreach (Transform child in block.transform)
            {
                Vector3Int position = TransformToMatrixPoints(child.transform.position);

                if (position.x >= 0 && position.x < grid.GetLength(0) && position.z >= 0 && position.z < grid.GetLength(1))
                {
                    solutionGrid[position.x, position.z] = 1;
                }
            }
  
        }
        MatrixInConsole(solutionGrid);
        bool equal = MatrixEqual(grid, solutionGrid);
        Debug.Log(equal);




    }

    bool MatrixEqual(int[,] matrix1, int[,] matrix2)
    {
 
        if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < matrix1.GetLength(0); i++) 
        {
            for (int j = 0; j < matrix1.GetLength(1); j++) 
            {
                if (matrix1[i, j] != matrix2[i, j])  
                {
                    
                    return false;
                }
            }
        }

        return true;
    }


    private Vector3Int TransformToMatrixPoints(Vector3 vector)
    {
        vector -= cellQuadObj.transform.position;
        vector *= 1 / cellSize;
        
        return Vector3Int.RoundToInt(vector);
    }


    private void MatrixInConsole(int[,] matrix)
    {
        String matrixString = "";
        for (int i = 0; i < matrix.GetLength(0); i++) 
        {
            for (int j = 0; j < matrix.GetLength(1); j++) 
            {
                matrixString += matrix[i, j] + " "; 
            }
            matrixString += "  ";
        }
        Debug.Log(matrixString);
    }

    private void LoadLevel(int level)
    {
        this.level = level;
        NewBlocks(level);
        NewGrid(level);
    }

    private void NewBlocks(int level)
    {
        // Blocks vom alten Level löschen
        foreach (GameObject block in activeBlocks)
        {
            Destroy(block);
        }
        activeBlocks.Clear();


        // Blocks für neues Level erstellen
        List<GameObject> blocksForLevel = levelBlocks[level];
        Vector3 startPosition = new Vector3(-10, 1, -1);
        float offsetZ = 1f;

        foreach (GameObject block in blocksForLevel)
        {
            GameObject blockForLevel = Instantiate(block, startPosition, Quaternion.identity);
            blockForLevel.SetActive(true);
            blockForLevel.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            activeBlocks.Add(blockForLevel);

            startPosition.z += offsetZ;
        }

    }

    private void NewGrid(int level)
    {
        // Grid-Elemente von altem Level löschen
        foreach (GameObject cellQuad in cellQuads)
        {
            Destroy(cellQuad);
        }
        cellQuads.Clear();

        foreach (GameObject gameQuad in gameQuads)
        {
            Destroy(gameQuad);
        }
        gameQuads.Clear();


        // Grid-Elemente für neues Level erstellen
        Vector3 gridCenter = transform.position;
        Renderer originalRenderer = cellQuadObj.GetComponent<Renderer>();
        originalRenderer.enabled = true;
        Renderer gameRenderer = gameQuadObj.GetComponent<Renderer>();
        gameRenderer.enabled = true;
        int[,] grid = levelGrids[level];

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
                    cellQuads.Add(newCellQuad);
                }
                else
                {
                    GameObject newGameQuad = Instantiate(gameQuadObj, centerPos + gridCenter, Quaternion.identity, transform);
                    newGameQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    newGameQuad.SetActive(true);
                    gameQuads.Add(newGameQuad);
                }


            }
        }

        originalRenderer.enabled = false;
        gameRenderer.enabled = false;
    }


    public void SetLevel(int index)
    {
        if (index == 0)
        {
           // Debug.Log("index 0");
            LoadLevel(1);
        }
        else if (index == 1)
        {
           // Debug.Log("index 1");
            LoadLevel(2);

        }
        else if (index == 2)
        {
           // Debug.Log("index 2");
            LoadLevel(3);

        }
        else if (index == 3)
        {
           // Debug.Log("Test");
            LoadLevel(4);

        }
        else
        {
            Debug.LogError("Ungültiger Index: " + index);
        }

        

        
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




    private void SnapToGrid(GameObject target)
    {
        // Debug.Log("SnapToGrid wurde aufgerufen!");
        // Runden der Position auf das nächste Grid
        float snapX = Mathf.Round(target.transform.position.x / cellSize) * cellSize;
        float snapY = Mathf.Round(target.transform.position.y / cellSize) * cellSize;
        float snapZ = Mathf.Round(target.transform.position.z / cellSize) * cellSize;

        // Setze die Position des Objekts auf das Grid
        target.transform.position = new Vector3(snapX, snapY, snapZ);
        Quaternion currentRotation = target.transform.rotation;
        Vector3 eulerRotation = currentRotation.eulerAngles;

        // Rotation auf bestimmte Winkel beschränken
        float snappedX = Mathf.Round(eulerRotation.x / 90) * 90;
        float snappedY = Mathf.Round(eulerRotation.y / 90) * 90;
        float snappedZ = Mathf.Round(eulerRotation.z / 90) * 90;

        target.transform.rotation = Quaternion.Euler(snappedX, snappedY, snappedZ);


        // Ausgabe für Debugging
        // Debug.Log($"Snapping to: {target.transform.position}");
        // Debug.Log($"Current rotation: {currentRotation.eulerAngles}");

        generalTarget = target;

    }

    // Diese Methode wird aufgerufen, wenn das Objekt losgelassen wird
    public void OnRelease(GameObject target)
    {
        SnapToGrid(target); 
    }
}