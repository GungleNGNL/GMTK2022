using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinding : Singleton<AIPathFinding>
{
    int gridHeight, gridWidth;
    private Node[,] grid; // astar

    public void GenerateAStarGrid(int height, int width)
    {
        gridHeight = height;
        gridWidth = width;
        grid = new Node[gridWidth, gridHeight];
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                grid[x, y] = new Node(new Vector2(x, y));
            }
        }
    }

    public Node GetGridNode(int x, int y)
    {
        return grid[x, y];
    }

    public Stack<Vector2> AStar(Vector2 startPos, Vector2 desPos , AIMoveType moveType)
    {
        int looplimit = 2000;
        int loopTime = 0;
        Node start = grid[(int)startPos.x, (int)startPos.y];
        Node des = grid[(int)desPos.x, (int)desPos.y];
        Stack<Vector2> path = new Stack<Vector2>();

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        Node current;

        start.g = 0;
        start.h = Vector2.Distance(startPos, desPos);
        open.Add(start);

        while (open.Count > 0)
        {
            if (loopTime > looplimit)
            {
                Debug.LogError("Loop more than loop limit");
                break;
            }
            current = open[0];
            for (int i = 1; i < open.Count; i++)
            {
                if (open[i].f < current.f || open[i].f == current.f)
                {
                    if (open[i].h < current.h)
                        current = open[i];
                }
            }
            if (current.pos == des.pos)
            {
                Stack<Vector2> desPath = ReconstructPath(start, des);
                return desPath;
            }
            open.Remove(current);
            closed.Add(current);
            List<Node> neighbourList = new List<Node>();

            switch (moveType)
            {
                case AIMoveType.oneToEightSide:
                    neighbourList = ChessBoard.Instance.GetOneToEightNeighbour(current.pos);
                    break;
                case AIMoveType.InfinityToEightSide:
                    neighbourList = ChessBoard.Instance.GetInfinityToEightNeighbour(current.pos);
                    break;
            }
            foreach (Node neighbour in neighbourList)
            {
                if (closed.Contains(neighbour))
                {
                    continue;
                }
                var cost = current.g + Vector2.Distance(current.pos, neighbour.pos);
                if (cost < neighbour.g || !open.Contains(neighbour))
                {
                    neighbour.g = cost;
                    neighbour.h = Vector2.Distance(neighbour.pos, desPos);
                    neighbour.parent = current;

                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                }
            }
            loopTime++;
        }
        return path;
    }

    Stack<Vector2> ReconstructPath(Node start, Node des)
    {
        Stack<Vector2> path = new Stack<Vector2>();
        Node currentNode = grid[(int)des.pos.x, (int)des.pos.y];
        path.Push(des.pos);
        while (currentNode.pos != start.pos)
        {
            if (currentNode.pos == des.pos)
            {
                currentNode = currentNode.parent;
                continue;
            }               
            path.Push(currentNode.pos);
            currentNode = currentNode.parent;
        }
        return path;
    }
}

public class Node
{
    public Vector2 pos;
    public Node parent;
    public float g, h;
    public Node(Vector2 pos)
    {
        this.pos = pos;
    }
    public float f
    {
        get
        {
            return g + h;
        }
    }
}

public enum AIMoveType
{
    oneToEightSide = 0,
    InfinityToEightSide = 1,
}
