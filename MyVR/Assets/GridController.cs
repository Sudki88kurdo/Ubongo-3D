using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.XR.Interaction.Toolkit;
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
    private List<GameObject> activeBlocksEnemy = new List<GameObject>();

    private Vector3 enemyGridStart = new Vector3(1, 0, 7);

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


    //To make it easier to access the script from other scripts
    public static GridController current;

    private List<GameObject> cellQuads; // Dynamische Liste
    private List<GameObject> gameQuads; // Dynamische Liste
    private List<GameObject> gameCubesColor;

    private List<GameObject> blocks;

    private GameObject generalTarget;

    /*
    private float previousY = -1;
    private float noMovementThreshold = 0.01f; 
    private float stationaryTime = 0f;
    private float stationaryTimeLimit = 0.5f;
    */

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


        levelBlocks[1] = new List<GameObject> { blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[2] = new List<GameObject> { blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[3] = new List<GameObject> { blockT, blockPlus, blockL, blockI, blockMinus, blockRhomb };
        levelBlocks[4] = new List<GameObject> { blockMinus, blockRhomb };
        levelBlocks[5] = new List<GameObject> { blockMinus, blockRhomb, blockT };
        levelBlocks[6] = new List<GameObject> { blockV, blockComplex1, blockComplex2, blockComplex3 };
        levelBlocks[7] = new List<GameObject> { blockPlus, blockMinus, blockV, blockComplex1 };

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
        //TestMovement();
        Boolean win = TestSolution(level);
        if (win && winState == WinState.Playing)
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

    /*
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
    }*/

    private Boolean TestSolution(int level)
    {
        int[,,] grid = levelGrids[level];
        //ToDo Teste ob die L�sung des Spielers richtig ist
        int[,,] solutionGrid = new int[grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)];


        foreach (GameObject block in activeBlocks)
        {

            foreach (Transform child in block.transform)
            {
                Vector3Int position = TransformToMatrixPoints(child.transform.position);


                if (position.y >= 0 && position.y < grid.GetLength(0) && position.x >= 0 && position.x < grid.GetLength(1) && position.z >= 0 && position.z < grid.GetLength(2))
                {
                    solutionGrid[position.y, position.x, position.z] = 1;
                    //Debug.Log(position);
                }
            }

        }
        bool equal = MatrixEqual(grid, solutionGrid);
        //Debug.Log(equal);
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
                for (int k = 0; k < matrix2.GetLength(2); k++)
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
        

        //SolveLevel(4);

        //SolveLevel(5);
        
        if(level >= 4)
        {
            NewBlocks(level, true);
            NewGrid(level, true);
            SolveLevel(level);
        } else
        {
            NewBlocks(level, false);
            NewGrid(level, false);
        }
        
    }


    private void SolveLevel(int level)
    {
        int[,,] gridOriginal = levelGrids[level];

        int[,,] grid = new int[gridOriginal.GetLength(0), gridOriginal.GetLength(1), gridOriginal.GetLength(2)];

        for (int i = 0; i < gridOriginal.GetLength(0); i++)
        {
            for (int j = 0; j < gridOriginal.GetLength(1); j++)
            {
                for (int k = 0; k < gridOriginal.GetLength(2); k++)
                {
                    grid[i, j, k] = gridOriginal[i, j, k];

                }
            }
        }

        Debug.Log("neues Grid");

        List<Block> voxelBlocks = new List<Block>();
        foreach (var block in activeBlocksEnemy)
        {
            int[,,] voxelMatrix = CreateVoxelMatrix(block);
            Block voxelBlock = new Block(voxelMatrix);
            voxelBlocks.Add(voxelBlock);
            Debug.Log("neuer Block");
        }

        Debug.Log("START SOLVING");

        /* StartCoroutine(SolveRecursion(grid, voxelBlocks, 0, result =>
         {
             Debug.Log("Gelöst: " + result);
         }));*/

        /* bool solved = SolveRecursion(grid, voxelBlocks, 0);

          Debug.Log("Gelöst: " + solved);*/

        /* Action<bool> onComplete = (success) =>
         {
             if (success)
             {
                 Debug.Log("Lösung gefunden!");
             }
             else
             {
                 Debug.Log("Keine Lösung gefunden.");
             }
         };

         // Rekursionsaufruf starten
         SolveRecursion(grid, voxelBlocks, 0, onComplete);*/

        //Debug.Log("Gelöst: "+solved);
        StartSolving(grid, voxelBlocks);


    }

    bool IsGridFull(int[,,] grid)
    {
        foreach (var value in grid)
        {
            if (value == 1) return false; // Es gibt noch unbedeckte Stellen.
        }
        return true;
    }

    // Die asynchrone Methode, die eine Task zurückgibt
    private async Task<bool> SolveRecursionAsync(int[,,] grid, List<Block> blocks, int blockIndex)
    {
        if (blockIndex == blocks.Count)
        {
            // TODO Lösung testen
            Debug.Log("Alles ausprobiert");
            return IsGridFull(grid);
        }

        Block block = blocks[blockIndex];

        // Wir verwenden hier `await Task.Yield()` um die Arbeit in kleineren Abschnitten durchzuführen
        // und dem Main Thread Zeit zu geben, andere Aufgaben zu erledigen.
        await Task.Yield(); // Hier wird der Kontext zurückgegeben, sodass der Hauptthread weiterläuft.

        for (int y = 0; y < grid.GetLength(0); y++)
        {
            Debug.Log("Teste neue Ebene " + y);
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    foreach (var rotatedBlock in block.GetAllRotations())
                    {
                        if (CheckPlacing(grid, rotatedBlock, x, y, z))
                        {
                            Debug.Log($"Platzierung möglich: Block {blockIndex} an Position ({x}, {y}, {z})");

                            PlaceBlock(grid, rotatedBlock, x, y, z, blockIndex);
                            Debug.Log(grid);
                            DebugPrintGrid(grid);

                            // Rekursiver Aufruf - Asynchron!
                            bool result = await SolveRecursionAsync(grid, blocks, blockIndex + 1);
                            if (result)
                            {
                                Debug.Log("Baustein passt");
                                return true;
                            }
                            RemoveBlock(grid, rotatedBlock, x, y, z, blockIndex);
                        }
                    }
                }
            }
        }
        return false;
    }

    // Aufruf der asynchronen Methode
    private void StartSolving(int[,,] grid, List<Block> blocks)
    {
        // Hier rufst du die asynchrone Methode auf
        SolveRecursionAsync(grid, blocks, 0).ContinueWith(task =>
        {
            // Nach Abschluss der Task kannst du hier auf das Ergebnis zugreifen
            if (task.Result)
            {
                Debug.Log("Lösung gefunden!");
            }
            else
            {
                Debug.Log("Keine Lösung gefunden.");
            }
        });
    }


    private bool SolveRecursion(int[,,] grid, List<Block> blocks, int blockIndex)
    {
        if (blockIndex == blocks.Count)
        {
            //TODO Lösung testen
            Debug.Log("Alles ausprobiert");
            return IsGridFull(grid);
        }

        Block block = blocks[blockIndex];
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            Debug.Log("Teste neue Ebene " + y);
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    foreach (var rotatedBlock in block.GetAllRotations())
                    {
                        if (CheckPlacing(grid, rotatedBlock, x, y, z))
                        {
                            Debug.Log($"Platzierung möglich: Block {blockIndex} an Position ({x}, {y}, {z})");

                            PlaceBlock(grid, rotatedBlock, x, y, z, blockIndex);
                            Debug.Log(grid);
                            DebugPrintGrid(grid);
                            if (SolveRecursion(grid, blocks, blockIndex + 1))
                            {
                                Debug.Log("Baustein passt");
                                return true;
                            }
                            RemoveBlock(grid, rotatedBlock, x, y, z, blockIndex);
                        }
                    }
                }
            }
        }
        return false;
    }
    
    /*
    private void SolveRecursion(int[,,] grid, List<Block> blocks, int blockIndex, Action<bool> onComplete)
    {
        // Basisfall: Wenn alle Blöcke ausprobiert wurden
        if (blockIndex == blocks.Count)
        {
            Debug.Log("Alles ausprobiert");
            onComplete(IsGridFull(grid));  // Ergebnis zurückgeben
            return;
        }

        Block block = blocks[blockIndex];
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            Debug.Log("Teste neue Ebene " + y);
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    foreach (var rotatedBlock in block.GetAllRotations())
                    {
                        if (CheckPlacing(grid, rotatedBlock, x, y, z))
                        {
                            Debug.Log($"Platzierung möglich: Block {blockIndex} an Position ({x}, {y}, {z})");

                            // Block platzieren und mit Rekursion fortfahren
                            PlaceBlock(grid, rotatedBlock, x, y, z, blockIndex);
                            Debug.Log(grid);
                            DebugPrintGrid(grid);

                            // Rekursive Methode aufrufen, ohne Yield
                            SolveRecursion(grid, blocks, blockIndex + 1, onComplete);

                            // Falls keine Lösung gefunden wird, Block wieder entfernen
                            RemoveBlock(grid, rotatedBlock, x, y, z, blockIndex);
                        }
                    }
                }
            }
        }

        // Falls keine Lösung gefunden wurde, Callback mit false aufrufen
        onComplete(false);
    }

    */

    /*

    private IEnumerator<object> SolveRecursion(int[,,] grid, List<Block> blocks, int blockIndex, Action<bool> onComplete)
    {
        if (blockIndex == blocks.Count)
        {
            //TODO Lösung testen
            Debug.Log("Alles ausprobiert");
            //return IsGridFull(grid);
            onComplete(IsGridFull(grid));
            yield break;
        }

        Block block = blocks[blockIndex];
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            Debug.Log("Teste neue Ebene "+ y);
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    foreach (var rotatedBlock in block.GetAllRotations())
                    {
                        if (CheckPlacing(grid, rotatedBlock, x, y, z))
                        {
                            Debug.Log($"Platzierung möglich: Block {blockIndex} an Position ({x}, {y}, {z})");

                            PlaceBlock(grid, rotatedBlock, x, y, z, blockIndex);
                            Debug.Log(grid);
                            DebugPrintGrid(grid);
                            yield return StartCoroutine(SolveRecursion(grid, blocks, blockIndex + 1, onComplete));
     
                            RemoveBlock(grid, rotatedBlock, x, y, z, blockIndex);
                        }
                    }
                }
            }
        }
        //return false;
        onComplete(false);
    }
*/

    private void DebugPrintGrid(int[,,] grid)
    {
        // Iteriere über alle Dimensionen des 3D-Arrays
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            string rowString = ""; // Für die Ausgabe einer Zeile
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                for (int z = 0; z < grid.GetLength(2); z++)
                {
                    rowString += grid[y, x, z] + " "; // Füge den Wert für jede Position in der Zeile hinzu
                }
                // Debug-Ausgabe nach jedem "Z" (eine Schicht im Gitter)
                Debug.Log(rowString);
                rowString = ""; // Zeilenstring zurücksetzen
            }
        }
    }

    private Vector3 TransformToEnemyWorldPoints(Vector3 vector)
    {
        
        vector *= cellSize;
        vector += enemyGridStart;

        return vector;
    }


    private void PlaceBlock(int[,,] grid, int[,,] block, int startX, int startY, int startZ, int blockIndex)
    {
        int index = 0;
        for (int y = 0; y < block.GetLength(0); y++)
        {
            for (int x = 0; x < block.GetLength(1); x++)
            {
                for (int z = 0; z < block.GetLength(2); z++)
                {
                    if (block[y, x, z] == 1)
                    {
                        grid[startY + y, startX + x, startZ + z] = 2;

                        //children[index].transform.position = new Vector3(x, y, z);
                        activeBlocksEnemy[blockIndex].transform.GetChild(index).position = TransformToEnemyWorldPoints(new Vector3(startX +x, startY+y, startZ+z));
                        Debug.Log("Vector vorher " + new Vector3(startX + x, startY + y, startZ + z));
                        Debug.Log("Vector hinterher " + TransformToEnemyWorldPoints(new Vector3(startX + x, startY + y, startZ + z)));
                        Debug.Log("Child of " + blockIndex);
                        Debug.Log(index);

                        index++;
                        //activeBlocksEnemy[blockIndex].transform.position = new Vector3(0, 0, 0);
                    }
                }
            }
        }
    }

    private void RemoveBlock(int[,,] grid, int[,,] block, int startX, int startY, int startZ, int blockIndex)
    {
        int index = 0;
        for (int y = 0; y < block.GetLength(0); y++)
        {
            for (int x = 0; x < block.GetLength(1); x++)
            {
                for (int z = 0; z < block.GetLength(2); z++)
                {
                    if (block[y, x, z] == 1)
                    {
                        grid[startY + y, startX + x, startZ + z] = 1;
                        

                        //activeBlocksEnemy[blockIndex].transform.GetChild(index).position = new Vector3(1, 1, 1);

                    }
                }
            }
        }
    }

    private bool CheckPlacing(int[,,] grid, int[,,] block, int startX, int startY, int startZ)
    {
        for (int y = 0; y < block.GetLength(0); y++)
        {
            for (int x = 0; x < block.GetLength(1); x++)
            {
                for (int z = 0; z < block.GetLength(2); z++)
                {
                    if (block[y, x, z] == 1)
                    {
                        int gridX = startX + x;
                        int gridY = startY + y;
                        int gridZ = startZ + z;

                        if (gridY >= grid.GetLength(0) || gridX >= grid.GetLength(1) || gridZ >= grid.GetLength(2) || grid[gridY, gridX, gridZ] != 1)
                        {
                            return false;
                        }   
                    }
                }
            }
        }
        return true;
    }

    private int[,,] CreateVoxelMatrix(GameObject block){
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;

        foreach (Transform voxel in block.transform)
        {
            Vector3 position = (block.transform.position - voxel.position) * (1/cellSize);

            minX = Mathf.Min(minX, (int)Math.Round(position.x, MidpointRounding.AwayFromZero));
            minY = Mathf.Min(minY, (int)Math.Round(position.y, MidpointRounding.AwayFromZero));
            minZ = Mathf.Min(minZ, (int)Math.Round(position.z, MidpointRounding.AwayFromZero));
            maxX = Mathf.Max(maxX, (int)Math.Round(position.x, MidpointRounding.AwayFromZero));
            maxY = Mathf.Max(maxY, (int)Math.Round(position.y, MidpointRounding.AwayFromZero));
            maxZ = Mathf.Max(maxZ, (int)Math.Round(position.z, MidpointRounding.AwayFromZero));
        }

        
        Debug.Log(minX+ " "+minY+ " "+minZ);
        int depth = Mathf.Max(1, maxX - minX + 1);
        int height = Mathf.Max(1, maxY - minY + 1);
        int width = Mathf.Max(1, maxZ - minZ + 1);

        int size = Mathf.Max(depth, height, width);

        int[,,] voxelMatrix = new int[size, size, size];

        int i = 0;
        foreach (Transform voxel in block.transform)
        {
            Vector3 position = (block.transform.position - voxel.position) * (1/cellSize);

            int x = (int)Math.Round(position.x - minX, MidpointRounding.AwayFromZero);
            int y = (int)Math.Round(position.y - minY, MidpointRounding.AwayFromZero);
            int z = (int)Math.Round(position.z - minZ, MidpointRounding.AwayFromZero);
            voxelMatrix[x, y, z] = 1;


            float fx = position.x - minX;
            float fy = position.y - minY;
            float fz = position.z - minZ;

            Debug.Log("vor runden " + fx+ " "+fy+" "+fz);
            Debug.Log("\n Voxel " + i + " Position "+ position +" "+ x+ " "+y+" " +z);

        }

        Debug.Log("CreateVoxelMatrix");
        DebugPrintGrid(voxelMatrix);
        Debug.Log("  Ende");
        return voxelMatrix;
    }


    class Block
    {
        public int[,,] Voxels { get; private set; }

        public Block(int[,,] voxels)
        {
            Voxels = voxels;
        }

        public List<int[,,]> GetAllRotations()
        {
            List <int[,,]> rotations = new List<int[,,]>();

            // Um jede Achse alle 4 Seiten
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for(int z = 0; z < 4; z++)
                    {
                        int[,,] rotatedVoxels = RotateVoxels(Voxels, x * 90, y * 90, z * 90);
                        if(!rotations.Any(r => AreEqual(r, rotatedVoxels)))
                        {
                            rotations.Add(rotatedVoxels);
                        }
                    }
                }
            }
            return rotations;
        }

        private bool AreEqual(int[,,] a, int[,,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1) || a.GetLength(2) != b.GetLength(2))
                return false;

            for (int x = 0; x < a.GetLength(0); x++)
                for (int y = 0; y < a.GetLength(1); y++)
                    for (int z = 0; z < a.GetLength(2); z++)
                        if (a[x, y, z] != b[x, y, z])
                            return false;

            return true;
        }
        /*
        public int[,,] RotateVoxels(int[,,] voxels, int x, int y, int z)
        {
            double xRadians = Math.PI * x / 180.0;
            double yRadians = Math.PI * y / 180.0;
            double zRadians = Math.PI * z / 180.0;

            // Rotationsmatrizen berechnen
            double[,] rotationX = {
                { 1, 0, 0 },
                { 0, Math.Cos(xRadians), -Math.Sin(xRadians) },
                { 0, Math.Sin(xRadians), Math.Cos(xRadians) }
            };

            double[,] rotationY = {
                { Math.Cos(yRadians), 0, Math.Sin(yRadians) },
                { 0, 1, 0 },
                { -Math.Sin(yRadians), 0, Math.Cos(yRadians) }
            };

            double[,] rotationZ = {
                { Math.Cos(zRadians), -Math.Sin(zRadians), 0 },
                { Math.Sin(zRadians), Math.Cos(zRadians), 0 },
                { 0, 0, 1 }
            };

            int xSize = voxels.GetLength(0);
            int ySize = voxels.GetLength(1);
            int zSize = voxels.GetLength(2);

            int[,,] rotatedVoxels = new int[xSize, ySize, zSize];

            for (int xi = 0; xi < xSize; xi++)
            {
                for (int yi = 0; yi < ySize; yi++)
                {
                    for (int zi = 0; zi < zSize; zi++)
                    {
                        if (voxels[xi, yi, zi] == 1)
                        {
                            
                            double[] rotatedVector = { xi, yi, zi };

                            rotatedVector = rotateVector(rotatedVector, rotationX);
                            rotatedVector = rotateVector(rotatedVector, rotationY);
                            rotatedVector = rotateVector(rotatedVector, rotationZ);

                            Debug.Log("rx "+rotatedVector[0]);
                            Debug.Log("ry "+rotatedVector[1]);
                            Debug.Log("rz "+rotatedVector[2]);

                            Debug.Log("ix "+(int)Math.Round(rotatedVector[0]));
                            Debug.Log("iy "+(int)Math.Round(rotatedVector[1]));
                            Debug.Log("iz "+(int)Math.Round(rotatedVector[2]));    

                            rotatedVoxels[(int)Math.Round(rotatedVector[0]), (int)Math.Round(rotatedVector[1]), (int)Math.Round(rotatedVector[2])] = 1;
                        }

                    }
                }
            }
            return rotatedVoxels;
        }*/




        public int[,,] RotateVoxels(int[,,] voxels, int x, int y, int z)
        {
            double xRadians = Math.PI * x / 180.0;
            double yRadians = Math.PI * y / 180.0;
            double zRadians = Math.PI * z / 180.0;

            // Rotationsmatrizen berechnen
            double[,] rotationX = {
        { 1, 0, 0 },
        { 0, Math.Cos(xRadians), -Math.Sin(xRadians) },
        { 0, Math.Sin(xRadians), Math.Cos(xRadians) }
    };

            double[,] rotationY = {
        { Math.Cos(yRadians), 0, Math.Sin(yRadians) },
        { 0, 1, 0 },
        { -Math.Sin(yRadians), 0, Math.Cos(yRadians) }
    };

            double[,] rotationZ = {
        { Math.Cos(zRadians), -Math.Sin(zRadians), 0 },
        { Math.Sin(zRadians), Math.Cos(zRadians), 0 },
        { 0, 0, 1 }
    };

            int xSize = voxels.GetLength(0);
            int ySize = voxels.GetLength(1);
            int zSize = voxels.GetLength(2);

            List<double[]> rotatedVectors = new List<double[]>();

            // Schritt 1: Alle rotieren und sammeln
            for (int xi = 0; xi < xSize; xi++)
            {
                for (int yi = 0; yi < ySize; yi++)
                {
                    for (int zi = 0; zi < zSize; zi++)
                    {
                        if (voxels[xi, yi, zi] == 1)
                        {
                            double[] rotatedVector = { xi, yi, zi };
                            rotatedVector = rotateVector(rotatedVector, rotationX);
                            rotatedVector = rotateVector(rotatedVector, rotationY);
                            rotatedVector = rotateVector(rotatedVector, rotationZ);

                            rotatedVectors.Add(rotatedVector);
                        }
                    }
                }
            }

            // Schritt 2: Minimalen Punkt berechnen
            double minX = rotatedVectors.Min(v => v[0]);
            double minY = rotatedVectors.Min(v => v[1]);
            double minZ = rotatedVectors.Min(v => v[2]);

            // Schritt 3: Normalisieren, um negative Werte zu vermeiden
            int normalizedXSize = (int)Math.Ceiling(rotatedVectors.Max(v => v[0] - minX)) + 1;
            int normalizedYSize = (int)Math.Ceiling(rotatedVectors.Max(v => v[1] - minY)) + 1;
            int normalizedZSize = (int)Math.Ceiling(rotatedVectors.Max(v => v[2] - minZ)) + 1;

            int[,,] rotatedVoxels = new int[normalizedXSize, normalizedYSize, normalizedZSize];

            // Schritt 4: Voxel in die normalisierte Matrix einfügen
            foreach (var vector in rotatedVectors)
            {
                int nx = (int)Math.Round(vector[0] - minX);
                int ny = (int)Math.Round(vector[1] - minY);
                int nz = (int)Math.Round(vector[2] - minZ);

                rotatedVoxels[nx, ny, nz] = 1;
            }

            return rotatedVoxels;
        }


        private double[] rotateVector(double[] vector, double[,] matrix)
        {
            return new double[]
            {
                    vector[0] * matrix[0, 0] + vector[1] * matrix[0, 1] + vector[2] * matrix[0, 2],
                    vector[0] * matrix[1, 0] + vector[1] * matrix[1, 1] + vector[2] * matrix[1, 2],
                    vector[0] * matrix[2, 0] + vector[1] * matrix[2, 1] + vector[2] * matrix[2, 2]
            };
        }
    }

    private void NewBlocks(int level, bool enemy)
    {
        // Blocks vom alten Level l�schen
        foreach (GameObject block in activeBlocks)
        {
            Destroy(block);
        }
        activeBlocks.Clear();

        foreach (GameObject block in activeBlocksEnemy)
        {
            Destroy(block);
        }
        activeBlocksEnemy.Clear();


        // Blocks f�r neues Level erstellen
        List<GameObject> blocksForLevel = levelBlocks[level];
        Vector3 startPosition = new Vector3(-10, 1, -1);
        Vector3 startPositionEnemy = new Vector3(-3, 1, 6);
        float offsetZ = 1f;

        foreach (GameObject block in blocksForLevel)
        {
            GameObject blockForLevel = Instantiate(block, startPosition, Quaternion.identity);
            blockForLevel.SetActive(true);
            blockForLevel.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            activeBlocks.Add(blockForLevel);

            startPosition.z += offsetZ;

            if(enemy)
            {
                GameObject blockForLevelEnemy = Instantiate(block, startPositionEnemy, Quaternion.identity);
                blockForLevelEnemy.SetActive(true);
                blockForLevelEnemy.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                XRGrabInteractable grabScript = blockForLevelEnemy.GetComponent<XRGrabInteractable>();
                if (grabScript != null)
                {
                    grabScript.enabled = false;
                }
                activeBlocksEnemy.Add(blockForLevelEnemy);

                Debug.Log("Enemy Block " + blockForLevelEnemy);

                startPositionEnemy.z += offsetZ;
            }
        }

    }

    private void NewGrid(int level, bool enemy)
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

                        if (enemy)
                        {
                            GameObject newCellQuadEnemy = Instantiate(cellQuadObj, centerPosSurface + enemyGridStart, Quaternion.identity, transform);
                            newCellQuadEnemy.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                            newCellQuadEnemy.SetActive(true);
                            cellQuads.Add(newCellQuadEnemy);
                        }
                    }
                    else if (grid[0, x, z] == 1)
                    {
                        GameObject newGameQuad = Instantiate(gameQuadObj, centerPosSurface + gridCenter, Quaternion.identity, transform);
                        newGameQuad.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                        newGameQuad.SetActive(true);
                        gameQuads.Add(newGameQuad);

                        if (enemy)
                        {
                            GameObject newGameQuadEnemy = Instantiate(gameQuadObj, centerPosSurface + enemyGridStart, Quaternion.identity, transform);
                            newGameQuadEnemy.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                            newGameQuadEnemy.SetActive(true);
                            gameQuads.Add(newGameQuadEnemy);
                        }
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
}