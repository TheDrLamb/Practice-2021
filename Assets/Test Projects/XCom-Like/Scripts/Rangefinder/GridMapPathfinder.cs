using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridMapPathfinder
{
    //Finds A* Path from a given point to a taget point.

    //[NOTE] -> Clean up so that a new Path Grid isnt created everytime a new path is needed. 
    //Instead create the PathGrid once on creation and simply reinitialize it when a new path is needed.

    //[Note] -> Add Diagonals for a better flow of movement
    public const int MOVE_COST = 10;
    public const int MOVE_COST_DIAG = 14;

    GridMap Map;
    PathNode[,] PathMap;

    public GridMapPathfinder(GridMap _Map)
    {
        Map = _Map;
        PathMap = new PathNode[Map.size, Map.size];
        Initialize();
    }

    public void Initialize()
    {
        //initialize grid
        for (int i = 0; i < Map.size; i++)
        {
            for (int j = 0; j < Map.size; j++)
            {
                PathNode newNode = new PathNode(Map.GetTile(i, j));
                newNode.g = 9999;
                newNode.parent = null;
                PathMap[i, j] = newNode;
            }
        }
    }

    public void Reset()
    {
        //initialize grid
        for (int i = 0; i < Map.size; i++)
        {
            for (int j = 0; j < Map.size; j++)
            {
                PathNode node = PathMap[i, j];
                node.g = 9999;
                node.parent = null;
            }
        }
    }


    public List<PathNode> FindPath(GridTile _start, GridTile _target)
    {

        Reset();

        PathNode startNode = PathMap[_start.X, _start.Y];
        PathNode endNode = PathMap[_target.X, _target.Y];

        List<PathNode> openList = new List<PathNode> { startNode };
        List<PathNode> closedList = new List<PathNode>();

        startNode.g = 0;
        startNode.h = CalculateDistance(startNode, endNode);

        while (openList.Count > 0)
        {
            PathNode pointer = GetLowestF(openList);
            if (pointer.tile == _target)
            {
                return CalculatePath(pointer);
            }
            openList.Remove(pointer);
            closedList.Add(pointer);

            foreach (PathNode neighborNode in GetNeighbors(pointer))
            {
                if (closedList.Contains(neighborNode)) continue;
                if (neighborNode.tile.occupied) continue; //Obstacle Avoidance

                int tempG = pointer.g + CalculateDistance(pointer, neighborNode);
                if (tempG < neighborNode.g)
                {
                    neighborNode.parent = pointer;
                    neighborNode.g = tempG;
                    neighborNode.h = CalculateDistance(neighborNode, endNode);

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDist = Mathf.Abs(a.X - b.X);
        int yDist = Mathf.Abs(a.Y - b.Y);
        int rDist = Mathf.Abs(xDist - yDist);
        return MOVE_COST * (xDist + yDist - rDist) + (rDist * MOVE_COST_DIAG);
    }

    private PathNode GetLowestF(List<PathNode> pathNodeList)
    {
        PathNode lowest = pathNodeList[0];
        foreach (PathNode node in pathNodeList)
        {
            if (node.f < lowest.f)
            {
                lowest = node;
            }
        }
        return lowest;
    }

    private List<PathNode> GetNeighbors(PathNode _currentNode)
    {
        List<PathNode> neighborList = new List<PathNode>();

        if (_currentNode.X - 1 >= 0)
        {
            //Get The left neighbor
            neighborList.Add(PathMap[_currentNode.X - 1, _currentNode.Y]);
            if (_currentNode.Y - 1 >= 0)
            {
                //Get The left down neighbor
                neighborList.Add(PathMap[_currentNode.X - 1, _currentNode.Y - 1]);
            }
            if (_currentNode.Y + 1 <= Map.size - 1)
            {
                //Get The left up neighbor 
                neighborList.Add(PathMap[_currentNode.X - 1, _currentNode.Y + 1]);
            }
        }
        if (_currentNode.X + 1 <= Map.size - 1)
        {
            //Get The right neighbor 
            neighborList.Add(PathMap[_currentNode.X + 1, _currentNode.Y]);
            if (_currentNode.Y - 1 >= 0)
            {
                //Get The right down neighbor
                neighborList.Add(PathMap[_currentNode.X + 1, _currentNode.Y - 1]);
            }
            if (_currentNode.Y + 1 <= Map.size - 1)
            {
                //Get The right up neighbor 
                neighborList.Add(PathMap[_currentNode.X + 1, _currentNode.Y + 1]);
            }
        }
        if (_currentNode.Y - 1 >= 0)
        {
            //Get The down neighbor
            neighborList.Add(PathMap[_currentNode.X, _currentNode.Y - 1]);
        }
        if (_currentNode.Y + 1 <= Map.size - 1)
        {
            //Get The up neighbor 
            neighborList.Add(PathMap[_currentNode.X, _currentNode.Y + 1]);
        }

        return neighborList;
    }

    private List<PathNode> CalculatePath(PathNode _target)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(_target);
        PathNode pointer = _target;
        while (pointer.parent != null)
        {
            path.Add(pointer.parent);
            pointer = pointer.parent;
        }
        path.Reverse();
        return path;
    }
}