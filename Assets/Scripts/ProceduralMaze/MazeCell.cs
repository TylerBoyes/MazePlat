using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeNode
{
    private Edge right;
    private Edge left;
    private Edge above;
    private Edge below;

    public Edge Right { get { return right; } set { right = value; } }
    public Edge Left { get { return left; } set { left = value; } }
    public Edge Above { get { return above; } set { above = value; } }
    public Edge Below { get { return below; } set { below = value; } }

    int X, Y;
    public int ConnectedNodes
    {
        get
        {
            int toReturn = 0;
            if (right != null && right.Connected)
                toReturn++;
            if (left != null && left.Connected)
                toReturn++;
            if (above != null && above.Connected)
                toReturn++;
            if (below != null && below.Connected)
                toReturn++;
            return toReturn;
        }
    }

    public System.Collections.Generic.IEnumerable<Edge> Edges
    {
        get
        {
            if (right != null)
                yield return right;
            if (left != null)
                yield return left;
            if (above != null)
                yield return above;
            if (below != null)
                yield return below;
        }
    }

    public MazeNode(int x, int y)
    {
        X = x;
        Y = y;
        right = null;
        left = null;
        above = null;
        below = null;
    }

    public override string ToString()
    {
        return "[" + X + "," + Y + "]";
    }
}
