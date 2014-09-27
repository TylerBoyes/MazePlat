using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prims : MazeType
{
    /// <summary>
    /// Uses a variation of Prims algorthm to select which edges are part of the minimum spanning tree
    /// http://en.wikipedia.org/wiki/Prim's_algorithm
    /// </summary>
    /// <param name="nodes">A complete weighted lattice graph</param>
    public override void GenerateMaze(MazeNode[,] nodes)
    {
        HashSet<MazeNode> visitedNodes = new HashSet<MazeNode>();
        while (true)
        {
            visitedNodes.Clear();
            Edge minimumEdge = FindMinimumEdge(nodes[0, 00], visitedNodes);
            if (minimumEdge == null)
            {
                return;
            }
            minimumEdge.Connected = true;
        }
    }

    /// <summary>
    /// Recursive function which traverses cells selecting the minimum edge in the graph
    /// </summary>
    Edge FindMinimumEdge(MazeNode node, HashSet<MazeNode> visitedNodes)
    {
        // If the cell is null, it's the edge of the graph. If we've already visited it, ignore it
        if (node == null || visitedNodes.Contains(node))
        {
            return null;
        }
        visitedNodes.Add(node);

        Edge minimumEdge = null;

        //For all surroundeding edges
        foreach (Edge edge in node.Edges)
        {
            if (edge != null)
            {
                Edge candidate = null;
                MazeNode otherNode = edge.MazeCellOne == node ? edge.MazeCellTwo : edge.MazeCellOne;

                if (edge.Connected)
                {
                    candidate = FindMinimumEdge(otherNode, visitedNodes);
                }
                else if (otherNode.ConnectedNodes <= 0) // if the otherNode isn't already in the tree, this edge is a candidate
                {
                    candidate = edge;
                }

                if (candidate != null)
                {
                    if (minimumEdge == null || candidate.Weight < minimumEdge.Weight)
                    {
                        minimumEdge = candidate;
                    }
                }
            }
        }

        return minimumEdge;
    }
}
