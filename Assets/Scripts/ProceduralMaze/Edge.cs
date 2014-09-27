using UnityEngine;
using System.Collections;

public class Edge
{
    public static int Seed = 0;
    public static int SeedIncrement = 1;

    private MazeNode cellOne;
    private MazeNode cellTwo;

    private float weight;
    private bool connected;

    public MazeNode MazeCellOne { get { return cellOne; } set { cellOne = value; } }
    public MazeNode MazeCellTwo { get { return cellTwo; } set { cellTwo = value; } }
    public bool Connected { get { return connected; } set { connected = value; } }
    public float Weight { get { return weight; } set { weight = value; } }

    public Edge(MazeNode cell, MazeNode cell2)
        : this(cell, cell2, false) { }

    public Edge(MazeNode cell1, MazeNode cell2, bool isConnected)
    {
        Random.seed = Seed;
        Seed += SeedIncrement;

        cellOne = cell1;
        cellTwo = cell2;
        connected = isConnected;
        weight = (Random.value * 1000.0f);
    }

    public override string ToString()
    {
        string result = "{" + MazeCellOne.ToString();
        result += "," + MazeCellTwo.ToString() + "(" + weight + ")}";
        return result;
    }
}