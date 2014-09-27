using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimumSpanningTree : MazeType
{
    public override void GenerateMaze(MazeNode[,] cells)
    {
        List<MazeNode> visitedCells = new List<MazeNode>();
        bool buildingTrees = true;
        while (buildingTrees)
        {
            visitedCells.Clear();
            Edge cheapestConnection = FindCheapestLink(cells);
            if (cheapestConnection == null)
                buildingTrees = false;
            else
                cheapestConnection.Connected = true;
        }

        //now, connect the trees
        visitedCells.Clear();
        HashSet<MazeNode> partOfMaze = new HashSet<MazeNode>();
        partOfMaze.Add(cells[0, 0]);
        foreach (MazeNode cell in partOfMaze)
        {
            foreach (Edge conn in cell.Edges)
            {
                if (conn.Connected)
                    visitedCells.Add(conn.MazeCellOne == cell ? conn.MazeCellTwo : conn.MazeCellOne);
            }
        }
        foreach (MazeNode cell in visitedCells)
            partOfMaze.Add(cell);

        List<Edge> neighbors = new List<Edge>();
        while (partOfMaze.Count < cells.Length)
        {
            //find all neighbors
            foreach (MazeNode cell in partOfMaze)
            {
                foreach (Edge conn in cell.Edges)
                {
                    if (!conn.Connected)
                        neighbors.Add(conn);
                }
            }

            //find cheapest connection
            Edge cheapest = neighbors[0];
            foreach (Edge conn in neighbors)
            {
                if (conn.Weight < cheapest.Weight)
                    cheapest = conn;
            }

            cheapest.Connected = true;
            partOfMaze.Add(partOfMaze.Contains(cheapest.MazeCellOne) ? cheapest.MazeCellTwo : cheapest.MazeCellOne);
        }
    }

    Edge FindCheapestLink(MazeNode[,] cells)
    {
        Edge cheapestConnection = null;
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(0); y++)
            {
                MazeNode cell = cells[x, y];
                if (cell.ConnectedNodes == 0)
                {
                    foreach (Edge conn in cell.Edges)
                    {
                        if (cheapestConnection == null || conn.Weight < cheapestConnection.Weight)
                            cheapestConnection = conn;
                    }
                }
            }
        }

        return cheapestConnection;
    }
}
