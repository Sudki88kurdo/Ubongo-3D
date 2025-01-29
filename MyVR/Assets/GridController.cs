using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GridController : MonoBehaviour
{
    //Gr��e eines Grids
    public float cellSize = 0.2f;

    //Quads um Grid anzuzeigen
    public GameObject cellQuadObj;
    public GameObject gameQuadObj;
    public GameObject gameCubeColorObj;

    // Erweiterung: Bei jedem Level werden Bausteine erzeugt
    public GameObject blockPattern5;
    public GameObject blockT;
    public GameObject blockPlus;
    public GameObject blockL;
    public GameObject blockI;
    public GameObject blockMinus;
    public GameObject blockRhomb;
    public GameObject blockV;
    public GameObject blockComplex1;
    public GameObject blockComplex2;
    public GameObject blockComplex3;

    public GameObject gameMenu;
    public GameObject winWindow;
    public Transform head;
    public float spawnDistanceMenu = 2f;
    public float spawnDistanceWindow = 1.5f;

    private Dictionary<int, List<GameObject>> levelBlocks = new Dictionary<int, List<GameObject>>();
    private List<GameObject> activeBlocks = new List<GameObject>();


    private Dictionary<int, int[,,]> levelGrids = new Dictionary<int, int[,,]>();

    private int level = 1;

    public enum WinState
    {
        Playing,
        Winning,
        WinWindow
    }

    public WinState winState;



    // Erweiterung: Automatische Erstellung von Grids anhand der Fl�chen der Bausteine
    //Game grid / Level
    int[,,] grid1 =
    {
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        },
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        }
    };



    int[,,] grid2 =
    {
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
        },
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        }
    };


    int[,,] grid3 =
    {
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
        },
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        }
    };

    int[,,] testGrid =
    {
        {
            {1, 1, 0, 0, 0},
            {1, 1, 0, 0, 0},
            {1, 1, 0, 0, 0},
            {0, 0, 0, 0, 0}
        },
        {
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0}
        }    
    };

    int[,,] testGrid3D =
    {
        {
            {1, 1, 0, 0, 0},
            {1, 1, 0, 0, 0},
            {1, 1, 0, 0, 0},
            {0, 0, 0, 0, 0}
        },
        {
            {1, 0, 0, 0, 0},
            {1, 1, 0, 0, 0},
            {1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0}
        }
    };

    int[,,] grid3D1 =
    {
        {
            {0, 1, 1, 0, 0},
            {0, 1, 1, 1, 0},
            {0, 1, 1, 1, 0},
            {0, 0, 1, 1, 1}
        },
        {
            {0, 0, 0, 0, 0},
            {0, 1, 1, 1, 0},
            {0, 1, 1, 0, 0},
            {0, 0, 0, 0, 1}
        }
    };

    int[,,] grid3D2 =
{
        {
            {1, 1, 1, 1, 1},
            {0, 1, 1, 1, 0},
            {0, 0, 1, 0, 0},
            {0, 0, 0, 0, 0}
        },
        {
            {0, 1, 0, 1, 1},
            {0, 1, 0, 1, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0}
        }
    };

     int[,,] grid3D3 =
    {
        {
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0},
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0},
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}


        },
        {
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0},
            {0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0},
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0},
            {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}


        }
    };



    //To make it easier to access the script from other scripts
    public static GridController current;

    private List<GameObject> cellQuads; // Dynamische Liste
    private List<GameObject> gameQuads; // Dynamische Liste
    private List<GameObject> gameCubesColor;

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
        gameCubesColor = new List<GameObject>();
        current = this;
        winState = WinState.Playing;

        blocks = new List<GameObject>();
        blocks.Add(blockPattern5);
        blocks.Add(blockT);
        blocks.Add(blockPlus);
        blocks.Add(blockL);
        blocks.Add(blockI);
        blocks.Add(blockMinus);
        blocks.Add(blockRhomb);
        blocks.Add(blockV);
        blocks.Add(blockComplex1);
        blocks.Add(blockComplex2);
        blocks.Add(blockComplex3);


        levelBlocks[1] = new List<GameObject> {  blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb, blockMinus, blockMinus };
        levelBlocks[2] = new List<GameObject> {  blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[3] = new List<GameObject> {  blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[4] = new List<GameObject> {  blockMinus, blockRhomb   };
        levelBlocks[5] = new List<GameObject> {  blockMinus, blockRhomb, blockT, blockPattern5 };
        levelBlocks[6] = new List<GameObject> {  blockV, blockComplex1, blockComplex2, blockComplex3 };
        levelBlocks[7] = new List<GameObject> {  blockPlus, blockMinus, blockV, blockComplex1 };
        for (int i = 8; i <= 10; i++){
            levelBlocks[i] = new List<GameObject> {  blockComplex3,blockT,blockI,blockV, blockPlus,blockL, blockT,blockL, blockComplex1,blockMinus,blockMinus, blockComplex2, blockRhomb,blockL };
            levelGrids[i] = grid3D3;
        }

        levelGrids[1] = grid1;
        levelGrids[2] = grid2;
        levelGrids[3] = grid3;
        levelGrids[4] = testGrid;
        levelGrids[5] = testGrid3D;
        levelGrids[6] = grid3D1;
        levelGrids[7] = grid3D2;
        


        LoadLevel(1);

        TestSolution(level);

    }

    void Update()
    {
        TestMovement();
        Boolean win = TestSolution(level);
        if(win && winState == WinState.Playing)
        {
            winState = WinState.WinWindow;
            // show win window and game menu
            gameMenu.SetActive(true);
            gameMenu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistanceMenu;
            gameMenu.transform.LookAt(new Vector3(head.position.x, gameMenu.transform.position.y, head.position.z));
            gameMenu.transform.forward *= -1;
            winWindow.SetActive(true);
            winWindow.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistanceWindow;
            winWindow.transform.LookAt(new Vector3(head.position.x, gameMenu.transform.position.y, head.position.z));
            winWindow.transform.forward *= -1;
        }
        if (!win)
        {
            winState = WinState.Playing;
        }
        
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

            // Wenn Baustein nicht mehr f�llt
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

    private Boolean TestSolution(int level)
    {
        int[,,] grid = levelGrids[level];
        //ToDo Teste ob die L�sung des Spielers richtig ist
        int[,,] solutionGrid = new int[grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)];


        foreach(GameObject block in activeBlocks)
        {

            foreach (Transform child in block.transform)
            {
                Vector3Int position = TransformToMatrixPoints(child.transform.position);


                if (position.y >= 0 && position.y < grid.GetLength(0) && position.x >= 0 && position.x < grid.GetLength(1) && position.z >= 0 && position.z < grid.GetLength(2))
                {
                    solutionGrid[position.y, position.x, position.z] = 1;
                    Debug.Log(position);
                }
            }
  
        }
        bool equal = MatrixEqual(grid, solutionGrid);
        Debug.Log(equal);
        return equal;




    }

    bool MatrixEqual(int[,,] matrix1, int[,,] matrix2)
    {
 
        if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < matrix1.GetLength(0); i++) 
        {
            for (int j = 0; j < matrix1.GetLength(1); j++) 
            {
                for(int k = 0; k < matrix2.GetLength(2); k++)
                {
                    if (matrix1[i, j, k] != matrix2[i, j, k])
                    {
                        return false;
                    }
                } 
            }
        }

        return true;
    }


    // Umwandeln von Koordinaten in Matrixpunkte
    private Vector3Int TransformToMatrixPoints(Vector3 vector)
    {
        vector -= cellQuadObj.transform.position;
        vector *= 1 / cellSize;
        
        return Vector3Int.RoundToInt(vector);
    }




private void LoadLevel(int level)
{
    this.level = level;
    if (level == 8)
    {
        int[,,] generatedGrid = GenerateRandomStructuredGrid(2, 5, 5, blocks);
        levelGrids[level] = generatedGrid;
        
    } 
     if(level > 8)
     {
        int[,,] generatedGrid = GenerateRandomStructuredGrid(3, 7, 7, blocks);
        levelGrids[level] = generatedGrid;
    }
    else
    {
        // Verwenden Sie die vorhandene Level-Generierung für Level unter 7
        levelGrids[level] = levelGrids[level];
    }
    NewBlocks(level);
    NewGrid(level);
}

    private void NewBlocks(int level)
    {
        // Blocks vom alten Level l�schen
        foreach (GameObject block in activeBlocks)
        {
            Destroy(block);
        }
        activeBlocks.Clear();


        // Blocks f�r neues Level erstellen
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
        // Grid-Elemente von altem Level l�schen
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

        foreach (GameObject gameCube in gameCubesColor)
        {
            Destroy(gameCube);
        }
        gameCubesColor.Clear();


        // Grid-Elemente f�r neues Level erstellen
        Vector3 gridCenter = transform.position;
        Renderer originalRenderer = cellQuadObj.GetComponent<Renderer>();
        originalRenderer.enabled = true;
        Renderer gameRenderer = gameQuadObj.GetComponent<Renderer>();
        gameRenderer.enabled = true;
        Renderer cubeColorRenderer = gameCubeColorObj.GetComponent<Renderer>();
        int[,,] grid = levelGrids[level];

        // Grid Elemente erstellen und anzeigen
        for (int y = 0;  y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    


                    Vector3 centerPosSurface = new Vector3(x * cellSize, 0, z * cellSize);

                    if (grid[0, x, z] == 0)
                    {
                        GameObject newCellQuad = Instantiate(cellQuadObj, centerPosSurface + gridCenter, Quaternion.identity, transform);
                        newCellQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        newCellQuad.SetActive(true);
                        cellQuads.Add(newCellQuad);
                    }
                    else if (grid[0, x, z] == 1)
                    {
                        GameObject newGameQuad = Instantiate(gameQuadObj, centerPosSurface + gridCenter, Quaternion.identity, transform);
                        newGameQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        newGameQuad.SetActive(true);
                        gameQuads.Add(newGameQuad);
                    }


                    Vector3 centerPosVolumeColor = new Vector3(x * cellSize, y * cellSize + cellSize / 2, z * cellSize + cellSize * (grid.GetLength(2)+1));

                    if (grid[y, x, z] == 1)
                    {
                        GameObject newGameCubeColor = Instantiate(gameCubeColorObj, centerPosVolumeColor + gridCenter, Quaternion.identity, transform);
                        newGameCubeColor.SetActive(true);
                        gameCubesColor.Add(newGameCubeColor);
                    }
                }
            }
        }
        

        originalRenderer.enabled = false;
        gameRenderer.enabled = false;
        cubeColorRenderer.enabled = false;

    }


    public void SetLevel(int index)
    {
        if(index < levelGrids.Count)
        {
            LoadLevel(index + 1);
        }
        else
        {
            Debug.LogError("Ung�ltiger Index: " + index);
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
        // Runden der Position auf das n�chste Grid
        float snapX = Mathf.Round(target.transform.position.x / cellSize) * cellSize;
        float snapY = Mathf.Round(target.transform.position.y / cellSize) * cellSize;
        float snapZ = Mathf.Round(target.transform.position.z / cellSize) * cellSize;

        // Setze die Position des Objekts auf das Grid
        target.transform.position = new Vector3(snapX, snapY, snapZ);
        Quaternion currentRotation = target.transform.rotation;
        Vector3 eulerRotation = currentRotation.eulerAngles;

        // Rotation auf bestimmte Winkel beschr�nken
        float snappedX = Mathf.Round(eulerRotation.x / 90) * 90;
        float snappedY = Mathf.Round(eulerRotation.y / 90) * 90;
        float snappedZ = Mathf.Round(eulerRotation.z / 90) * 90;

        target.transform.rotation = Quaternion.Euler(snappedX, snappedY, snappedZ);


        // Ausgabe f�r Debugging
        // Debug.Log($"Snapping to: {target.transform.position}");
        // Debug.Log($"Current rotation: {currentRotation.eulerAngles}");

        generalTarget = target;

    }

    // Diese Methode wird aufgerufen, wenn das Objekt losgelassen wird
    public void OnRelease(GameObject target)
    {
        SnapToGrid(target); 
    }

private int[,,] GenerateRandomStructuredGrid(int depth, int width, int height, List<GameObject> blocks)
{
    int[,,] grid = new int[depth, width, height];
    System.Random rand = new System.Random();

    // Sicherstellen, dass mindestens zwei benachbarte Blöcke auf der ersten Ebene platziert werden
    int startX = rand.Next(0, width - 1);  // Mindestens zwei nebeneinander
    int startZ = rand.Next(1, height);     // Zufällige Höhe
    grid[0, startX, startZ] = 1;          // Erster Block
    grid[0, startX + 1, startZ] = 1;      // Zweiter Block, nebeneinander

    // Liste der verbundenen Blöcke mit der Position der beiden ersten Blöcke
    List<Vector3Int> connectedBlocks = new List<Vector3Int>
    {
        new Vector3Int(startX, 0, startZ),
        new Vector3Int(startX + 1, 0, startZ)
    };

    // Platzierung der restlichen Blöcke
    foreach (var block in blocks)
    {
        bool placed = false;
        int attempts = 0;

        while (!placed && attempts < 100)  // Versuche, den Block zu platzieren
        {
            // Wähle einen zufälligen verbundenen Block aus der Liste
            Vector3Int connectedBlock = connectedBlocks[rand.Next(connectedBlocks.Count)];
            List<Vector3Int> possiblePositions = new List<Vector3Int>();

            // Überprüfen alle benachbarten Positionen (oben, unten, links, rechts, vorne, hinten)
            foreach (Vector3Int direction in new Vector3Int[] {
                new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1),
                new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0)
            })
            {
                Vector3Int newPosition = connectedBlock + direction;

                if (newPosition.x >= 0 && newPosition.x < width && newPosition.y >= 0 && newPosition.y < depth && newPosition.z >= 0 && newPosition.z < height && grid[newPosition.y, newPosition.x, newPosition.z] == 0)
                {
                    possiblePositions.Add(newPosition);
                }
            }

            // Wenn es mindestens zwei benachbarte Positionen gibt, die leer sind
            if (possiblePositions.Count >= 2)
            {
                // Wähle zufällig eine der möglichen Positionen aus
                Vector3Int chosenPosition = possiblePositions[rand.Next(possiblePositions.Count)];
                grid[chosenPosition.y, chosenPosition.x, chosenPosition.z] = 1;
                connectedBlocks.Add(chosenPosition);  // Der Block wird der Liste der verbundenen Blöcke hinzugefügt
                placed = true;
            }
            attempts++;
        }
    }

    // Stellen sicher, dass alle Blöcke miteinander verbunden sind
    EnsureConnectivity(grid, width, height, depth);

    return grid;
}

// Hilfsfunktion, um die Verbindung der Blöcke zu überprüfen und sicherzustellen, dass alle zusammenhängend sind
private void EnsureConnectivity(int[,,] grid, int width, int height, int depth)
{
    bool[,,] visited = new bool[depth, width, height];
    Queue<Vector3Int> queue = new Queue<Vector3Int>();

    // Suche nach dem ersten Block (den ersten befüllten Block)
    for (int y = 0; y < depth; y++)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (grid[y, x, z] == 1)
                {
                    queue.Enqueue(new Vector3Int(x, y, z));
                    visited[y, x, z] = true;
                    break;
                }
            }
        }
    }

    // Führe eine Breitensuche durch, um alle erreichbaren Blöcke zu besuchen
    while (queue.Count > 0)
    {
        Vector3Int current = queue.Dequeue();

        foreach (Vector3Int direction in new Vector3Int[] {
            new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1),
            new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0)
        })
        {
            Vector3Int neighbor = current + direction;

            if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < depth && neighbor.z >= 0 && neighbor.z < height && grid[neighbor.y, neighbor.x, neighbor.z] == 1 && !visited[neighbor.y, neighbor.x, neighbor.z])
            {
                visited[neighbor.y, neighbor.x, neighbor.z] = true;
                queue.Enqueue(neighbor);
            }
        }
    }

    // Überprüfen, ob alle Blöcke erreicht wurden
    for (int y = 0; y < depth; y++)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (grid[y, x, z] == 1 && !visited[y, x, z])
                {
                    // Falls ein Block nicht verbunden ist, platziere ihn so, dass er verbunden wird
                    grid[y, x, z] = 1;
                }
            }
        }
    }
}


    
}