using UnityEngine;
using System.Collections;

public class MazeGenerator : MonoBehaviour 
{
    public GameObject Player;

    const float BLOCK_SIZE = 1;
    const int CELL_SIZE = 4;

    public int Seed = 0;
    public int SeedIncrement = 1;
    public int MazeSizeX = 4;
    public int MazeSizeY = 4;

    public string MazeType;
    private MazeType mazeType;

    public string LeftEndPrefab = "MetalBeams/LeftEndBeam";
    public string LeftEndFlatPrefab = "MetalBeams/LeftEndFlatBeam";
    public string RightEndPrefab = "MetalBeams/RightEndBeam";
    public string HorizontalPrefab = "MetalBeams/HorizontalBeam";
    public string VerticalPrefab = "MetalBeams/VerticleBeam";
    public string TopEndPrefab = "MetalBeams/TopEndBeam";
    public string TopEndPrefabFlat = "MetalBeams/TopEndFlatBeam";
    public string BottomEndPrefab = "MetalBeams/BottomEndBeam";
    public string DoorPrefab = "MazeDoor";

    private MazeNode[,] cells;

	// Use this for initialization
	void Start ()
    {
        Edge.Seed = Seed;
        Edge.SeedIncrement = SeedIncrement;
        cells = new MazeNode[MazeSizeX, MazeSizeY];

        CreateCellArray();

        PopulateCellConnections();

        if (MazeType == "MinTree" || MazeType == "MinimumSpanningTree")
        {
            mazeType = new MinimumSpanningTree();
        }
        else if (MazeType == "Prims")
        {
            mazeType = new Prims();
        }
        else
        {
            mazeType = new DefaultMazeType();
        }
        mazeType.GenerateMaze(cells);

        BuildMaze();
        //BuildMazeArray();

        //we need to set the players position :)
        float startX = MazeSizeY % 2 == 0 ? 2 : 0;
        float startY = ((MazeSizeY - 1) * (CELL_SIZE - 1) - 2) * BLOCK_SIZE;
        Vector3 startPos = new Vector3(startX, startY, 0);
        Player.transform.position = startPos;

        //add entrance door

        GameObject entranceDoor = (GameObject)Instantiate(Resources.Load(DoorPrefab));
        entranceDoor.transform.position = startPos;
    }

    private void BuildMazeArray()
    {
    }

    private void BuildMaze()
    {
        //Create top
        for (int x = 0; x < MazeSizeX; x++)
        {
            int BlocksPerCell = 4;
            Vector3 cellPosition = new Vector3(
                x * BLOCK_SIZE * BlocksPerCell,
                BLOCK_SIZE * (CELL_SIZE - 1) * (MazeSizeY - 1),
                0);

            string platformType = HorizontalPrefab;

            for (int i = 0; i < 4; i++)
            {
                Vector3 blockOffset = new Vector3(i * BLOCK_SIZE, 0);
                GameObject leftPlat = (GameObject)Instantiate(Resources.Load(platformType));
                leftPlat.transform.position = cellPosition + blockOffset;
            }
        }

        //Create left side
        for (int y = 0; y < MazeSizeY; y++)
        {
            Vector3 cellPosition = new Vector3(
                -BLOCK_SIZE,
                (y - 1) * (CELL_SIZE - 1),
                0);

            string platformType = HorizontalPrefab;

            for (int i = 0; i < CELL_SIZE - 1; i++)
            {
                Vector3 blockOffset = new Vector3(0, (i + 1) * BLOCK_SIZE);
                GameObject leftPlat = (GameObject)Instantiate(Resources.Load(platformType));
                leftPlat.transform.position = cellPosition + blockOffset;
            }
        }
        GameObject corner = (GameObject)Instantiate(Resources.Load(HorizontalPrefab));
        corner.transform.position = new Vector3(-BLOCK_SIZE, -(CELL_SIZE - 1) * BLOCK_SIZE);

        for (int x = 0; x < MazeSizeX; x++)
        {
            for (int y = 0; y < MazeSizeY; y++)
            {
                Vector3 cellPosition = new Vector3(
                    x * BLOCK_SIZE * CELL_SIZE,
                    y * BLOCK_SIZE * (CELL_SIZE - 1),
                    0);

                MazeNode cell = cells[x, y];

                string platformType = HorizontalPrefab;

                //BottomLayer
                bool makeBottom = cell.Below == null || !cell.Below.Connected;
                if (makeBottom)
                {
                    for (int i = 1; i < CELL_SIZE - 2; i++)
                    {
                        Vector3 blockOffset = new Vector3(i * BLOCK_SIZE, -(CELL_SIZE - 1) * BLOCK_SIZE);
                        GameObject leftPlat = (GameObject)Instantiate(Resources.Load(platformType));
                        leftPlat.transform.position = cellPosition + blockOffset;
                    }
                }

                if (makeBottom || y % 2 == 0)
                {
                    Vector3 offset = new Vector3(0 * BLOCK_SIZE, -(CELL_SIZE - 1) * BLOCK_SIZE);
                    GameObject block = (GameObject)Instantiate(Resources.Load(platformType));
                    block.transform.position = cellPosition + offset;
                }
                if (makeBottom || y % 2 == 1)
                {
                    Vector3 offset = new Vector3((CELL_SIZE - 2) * BLOCK_SIZE, -(CELL_SIZE - 1) * BLOCK_SIZE);
                    GameObject block = (GameObject)Instantiate(Resources.Load(platformType));
                    block.transform.position = cellPosition + offset;
                }


                if (cell.Right == null || !cell.Right.Connected)
                {
                    //Right Layer                    
                    for (int i = 0; i < CELL_SIZE - 2; i++)
                    {
                        Vector3 blockOffset = new Vector3(CELL_SIZE - 1 * BLOCK_SIZE, -(i + 1) * BLOCK_SIZE);
                        GameObject leftPlat = (GameObject)Instantiate(Resources.Load(platformType));
                        leftPlat.transform.position = cellPosition + blockOffset;
                    }                     
                }
                //we always generate corner blocks
                Vector3 offsetB = new Vector3(CELL_SIZE - 1 * BLOCK_SIZE, -(CELL_SIZE - 1 * BLOCK_SIZE));
                GameObject blockB = (GameObject)Instantiate(Resources.Load(platformType));
                blockB.transform.position = cellPosition + offsetB;
            }
        }
    }


    private void PopulateCellConnections()
    {
        for (int x = 0; x < MazeSizeX; x++)
        {
            for (int y = 0; y < MazeSizeY; y++)
            {
                MazeNode cell = cells[x, y];
                if (x > 0)
                {
                    MazeNode otherCell = cells[x - 1, y];
                    if (otherCell.Right == null)
                    {
                        cell.Left = new Edge(otherCell, cell);
                        otherCell.Right = cell.Left;
                    }
                }
                if (x < MazeSizeX - 1)
                {
                    MazeNode otherCell = cells[x + 1, y];
                    if (otherCell.Left == null)
                    {
                        cell.Right = new Edge(otherCell, cell);
                        otherCell.Left = cell.Right;
                    }
                }
                if (y > 0)
                {
                    MazeNode otherCell = cells[x, y - 1];
                    if (otherCell.Above == null)
                    {
                        cell.Below = new Edge(otherCell, cell);
                        otherCell.Above = cell.Below;
                    }
                }
                if (y < MazeSizeY - 1)
                {
                    MazeNode otherCell = cells[x, y + 1];
                    if (otherCell.Below == null)
                    {
                        cell.Above = new Edge(otherCell, cell);
                        otherCell.Below = cell.Above;
                    }
                }
            }
        }
    }

    private void CreateCellArray()
    {
        for (int x = 0; x < MazeSizeX; x++)
        {
            for (int y = 0; y < MazeSizeY; y++)
            {
                MazeNode cell = new MazeNode(x, y);
                cells[x, y] = cell;
            }
        }
    }

    // Update is called once per frame
    void Update() 
    {
	
	}
}
