using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGeneratorOld : MonoBehaviour 
{
    public GameObject player;

    public int Seed;
    public int Size;
    int sizeX;
    int sizeY;

    public string LeftEndPrefab = "MetalBeams/LeftEndBeam";
    public string LeftEndFlatPrefab = "MetalBeams/LeftEndFlatBeam";
    public string RightEndPrefab = "MetalBeams/RightEndBeam";
    public string HorizontalPrefab = "MetalBeams/HorizontalBeam";
    public string VerticalPrefab = "MetalBeams/VerticleBeam";
    public string TopEndPrefab = "MetalBeams/TopEndBeam";
    public string TopEndPrefabFlat = "MetalBeams/TopEndFlatBeam";
    public string BottomEndPrefab = "MetalBeams/BottomEndBeam";

    bool[][] maze;
	// Use this for initialization
	void Start () 
    {
        sizeX = Size * 4;
        sizeY = Size * 3 - 1;

        CreateBoxWallsHorizontal();

        CreateBoxWallsVertical();

        CreatePlayerStartLocaion();

        GenerateMaze();
        FillInMaze();
	}

    private void GenerateMaze()
    {
        //a float array representing connections on a graph
        float[][] weightedEdge = new float[Size][];
        for (int i = 0; i < weightedEdge.Length; i++)
        {
            weightedEdge[i] = new float[Size];
            for (int j = 0; j < weightedEdge[i].Length; j++)
            {
                Random.seed = Seed++;
                weightedEdge[i][j] = Random.value;
            }
        }

        bool[][] activeEdges = new bool[Size][];
        for (int i = 0; i < activeEdges.Length; i++)
        {
            activeEdges[i] = new bool[Size - 1];
            for (int j = 0; j < activeEdges[i].Length; j++)
            {
                activeEdges[i][j] = false;
            }
        }

        bool[][] connected = new bool[Size][];
        for (int i = 0; i < connected.Length; i++)
        {
            connected[i] = new bool[Size];
            for (int j = 0; j < connected[i].Length; j++)
            {
                connected[i][j] = false;
            }
        }

        connected[0][0] = true;
        ArrayList arrayList = new ArrayList();
        List<IntPair> connectedPairs = new List<IntPair>();
        connectedPairs.Add(new IntPair(0, 0));

        while (connectedPairs.Count < (Size * Size))
        {
            IntPair fromPair = new IntPair(0,0);
            IntPair cheapestNode = new IntPair(0,0);
            float cheapestCost = 10;
            foreach (IntPair pair in connectedPairs)
            {
                //print("Checking {" + pair.X + "," + pair.Y + "}");

                if (pair.X > 0)
                {
                    int xCheck = pair.X-1;
                    int yCheck = pair.Y;
                    if (!connected[xCheck][yCheck])
                    {
                        float cost = GetCost(weightedEdge, pair.X, pair.Y, xCheck, yCheck);
                        if (cost < cheapestCost)
                        {
                            cheapestCost = cost;
                            cheapestNode = new IntPair(xCheck, yCheck);
                            fromPair = pair;
                        }
                    }
                }
                if (pair.X < Size - 1)
                {
                    int xCheck = pair.X + 1;
                    int yCheck = pair.Y;
                    if (!connected[xCheck][yCheck])
                    {
                        float cost = GetCost(weightedEdge, pair.X, pair.Y, xCheck, yCheck);
                        if (cost < cheapestCost)
                        {
                            cheapestCost = cost;
                            cheapestNode = new IntPair(xCheck, yCheck);
                            fromPair = pair;
                        }
                    }
                }
                if (pair.Y > 0)
                {
                    int xCheck = pair.X;
                    int yCheck = pair.Y - 1;
                    if (!connected[xCheck][yCheck])
                    {
                        float cost = GetCost(weightedEdge, pair.X, pair.Y, xCheck, yCheck);
                        if (cost < cheapestCost)
                        {
                            cheapestCost = cost;
                            cheapestNode = new IntPair(xCheck, yCheck);
                            fromPair = pair;
                        }
                    }
                }
                if (pair.Y < Size - 1)
                {
                    int xCheck = pair.X;
                    int yCheck = pair.Y + 1;
                    if (!connected[xCheck][yCheck])
                    {
                        float cost = GetCost(weightedEdge, pair.X, pair.Y, xCheck, yCheck);
                        if (cost < cheapestCost)
                        {
                            cheapestCost = cost;
                            cheapestNode = new IntPair(xCheck, yCheck);
                            fromPair = pair;
                        }
                    }
                }
            }

            SetNode(activeEdges, cheapestNode, fromPair);

            print("Added {" + cheapestNode.X + "," + cheapestNode.Y + "}");
            connected[cheapestNode.X][cheapestNode.Y] = true;
            connectedPairs.Add(cheapestNode);
        }



        // A boolean array representing connections between nodes on a graph

        // A boolean array representing the actual maze blocks
        maze = new bool[sizeX-1][];
        for (int i = 0; i < maze.Length; i++)
        {
            maze[i] = new bool[sizeY];
            for (int j = 0; j < maze[i].Length; j++)
            {
                int mazeIndexX = 0;
                int mazeIndexY = 0;

                if (j % 2 == 1)
                {
                    mazeIndexY = Mathf.Max(0, (j / sizeY - 1));
                    mazeIndexX = i / 3;
                }
                else
                {
                    mazeIndexY = j / 4;
                    mazeIndexX = Mathf.Max(0, (i / sizeX - 1));
                }
                mazeIndexX = Mathf.Min(activeEdges.Length - 1, Mathf.Max(0, mazeIndexX));
                mazeIndexY = Mathf.Min(activeEdges[0].Length - 1, Mathf.Max(0, mazeIndexY));

                if ((j + 1) % 3 == 0 || (i + 1) % 4 == 0 && !activeEdges[mazeIndexX][mazeIndexY])
                    maze[i][j] = true;
                else
                    maze[i][j] = false;

                maze[i][j] = !activeEdges[mazeIndexX][mazeIndexY];
               // maze[i][j] = !activeEdges[i][j];
            }
        }

    }

    void SetNode(bool[][] connections, IntPair to, IntPair from)
    {
        //print("Getting cost {" + x + "," + y + "}, {" + checkX + "," + checkY + "}");

        if (to.X > from.X)
            connections[from.X][from.X] = true;
        else if (to.X < from.X)
            connections[to.X][to.X] = true;
        else if (to.Y > from.Y)
            connections[from.X][from.Y] = true;
        else if (to.Y < from.Y)
            connections[to.X][to.Y] = true;
    }

    float GetCost(float[][] weightedEdge, int x, int y, int checkX, int checkY)
    {

        //print("Getting cost {" + x + "," + y + "}, {" + checkX + "," + checkY + "}");

        if (x > checkX)
            return weightedEdge[checkX][checkY];
        else if (x < checkX)
            return weightedEdge[x][y];
        else if (y > checkY)
            return weightedEdge[checkX][checkY];
        else if (y < checkY)
            return weightedEdge[x][y];

        return 10;
    }

    IntPair CheckLocation(int x, int y, bool[][] connected, float[][] weightedEdge, IntPair cheapestNode, float cheapestCost, out float newCost)
    {

        newCost = cheapestCost;
        return cheapestNode;
    }

    private void FillInMaze()
    {
        for (int i = 0; i < maze.Length; i++)
        {
            for (int j = 0; j < maze[i].Length; j++)
            {
                FillBlock(i, j);
            }
        }
    }

    private void FillBlock(int i, int j)
    {
        if (maze[i][j])
        {
            string platformType = HorizontalPrefab;
            if (i == 0)
            {
                if (maze[i + 1][j])
                    platformType = HorizontalPrefab;
                else
                    platformType = RightEndPrefab;
            }
            else if (i == maze.Length - 1)
            {
                if (maze[i - 1][j])
                    platformType = HorizontalPrefab;
                else
                    platformType = LeftEndPrefab;
            }
            else if (j == 0)
            {
                if (maze[i][j + 1])
                    platformType = VerticalPrefab;
                else
                    platformType = BottomEndPrefab;
            }
            else if (j == maze[i].Length - 1)
            {
                if (maze[i][j - 1])
                    platformType = VerticalPrefab;
                else
                    platformType = TopEndPrefab;
            }
            else if (maze[i + 1][j] && maze[i - 1][j])
                platformType = HorizontalPrefab;
            else if (maze[i + 1][j] && !maze[i - 1][j])
                platformType = LeftEndPrefab;
            else if (!maze[i + 1][j] && maze[i - 1][j])
                platformType = RightEndPrefab;
            else if (maze[i][j - 1] && maze[i][j + 1])
                platformType = VerticalPrefab;

            GameObject leftPlat = (GameObject)Instantiate(Resources.Load(platformType));
            leftPlat.transform.position = new Vector3(i - sizeX / 2 + 1, j - sizeY / 2 + 1, 0);

        }
    }

    private void CreatePlayerStartLocaion()
    {
        if (Size % 2 == 0)
            player.transform.position = new Vector3(-sizeX / 2 + 1, sizeY / 2 + 0.5f, 0);
        else
            player.transform.position = new Vector3(-sizeX / 2 + 1, sizeY / 2, 0);
    }

    private void CreateBoxWallsHorizontal()
    {
        for (int i = 0; i < sizeX + 1; i++)
        {
            string prefabName = HorizontalPrefab;
            if (i == 0)
                prefabName = LeftEndPrefab;
            else if (i == sizeX)
                prefabName = RightEndPrefab;

            float xPos = i - sizeX / 2;
            float yPos = sizeY / 2;
            var instob = Instantiate(Resources.Load(prefabName));
            GameObject go = (GameObject)instob;
            if (Size%2 == 0)
                go.transform.position = new Vector3(xPos, yPos + 2, 0);
            else
                go.transform.position = new Vector3(xPos, yPos + 1, 0);

            instob = Instantiate(Resources.Load(prefabName));
            go = (GameObject)instob;
            go.transform.position = new Vector3(xPos, -yPos, 0);
        }
    }

    private void CreateBoxWallsVertical()
    {
        for (int i = 1; i < sizeY+1; i++)
        {
            string prefabName = VerticalPrefab;
            if (i == 1)
                prefabName = BottomEndPrefab;
            else if (i == sizeY)
                prefabName = TopEndPrefabFlat;

            float xPos = sizeX / 2;
            float yPos = i - sizeY / 2;

            var instob = Instantiate(Resources.Load(prefabName));
            GameObject go = (GameObject)instob;
            go.transform.position = new Vector3(xPos, yPos, 0);

            instob = Instantiate(Resources.Load(prefabName));
            go = (GameObject)instob;
            go.transform.position = new Vector3(-xPos, yPos, 0);
        }
    }

    // Update is called once per frame
    void Update() 
    {
	
	}
}

struct IntPair
{
    public int X;
    public int Y;

    public IntPair(int x, int y)
    {
        X = x;
        Y = y;
    }
}
