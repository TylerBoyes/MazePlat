using UnityEngine;
using System.Collections;

public class MazeType
{
    public virtual void GenerateMaze(MazeNode[,] cells)
    {
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                MazeNode cell = cells[x, y];

                if (cell.Right != null)
                {
                    cell.Right.Connected = true;
                    cell.Right.MazeCellOne.Left.Connected = true;
                }

                if (cell.Above != null)
                {
                    cell.Above.Connected = true;
                    cell.Above.MazeCellOne.Below.Connected = true;
                }
            }
        }
    }
}
