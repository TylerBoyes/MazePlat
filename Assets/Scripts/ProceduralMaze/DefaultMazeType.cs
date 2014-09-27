using UnityEngine;
using System.Collections;

public class DefaultMazeType : MazeType
{
    public override void GenerateMaze(MazeNode[,] cells)
    {
        System.Random rand = new System.Random();
        if (rand.NextDouble() > 0.5)
            GenerateHorizontal(cells);
        else
            GenerateVertical(cells);
    }

    private static void GenerateVertical(MazeNode[,] cells)
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                MazeNode cell = cells[x, y];
                bool noSide = false;
                if (x % 2 == 1)
                {
                    if (y == cells.GetLength(1) - 1)
                    {
                        noSide = true;
                    }
                }
                else
                {
                    if (y == 0)
                    {
                        noSide = true;
                    }
                }

                if (cell.Above != null)
                {
                    cell.Above.Connected = true;
                    cell.Above.MazeCellOne.Below.Connected = true;
                }

                if (noSide && cell.Right != null)
                {
                    cell.Right.Connected = true;
                    cell.Right.MazeCellOne.Left.Connected = true;
                }

            }
        }
    }

    private static void GenerateHorizontal(MazeNode[,] cells)
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                MazeNode cell = cells[x, y];
                bool noRoof = false;
                if (y % 2 == 1)
                {
                    if (x == cells.GetLength(0) - 1)
                    {
                        noRoof = true;
                    }
                }
                else
                {
                    if (x == 0)
                    {
                        noRoof = true;
                    }
                }

                if (noRoof && cell.Above != null)
                {
                    cell.Above.Connected = true;
                    cell.Above.MazeCellOne.Below.Connected = true;
                }

                if (cell.Right != null)
                {
                    cell.Right.Connected = true;
                    cell.Right.MazeCellOne.Left.Connected = true;
                }

            }
        }
    }
}
