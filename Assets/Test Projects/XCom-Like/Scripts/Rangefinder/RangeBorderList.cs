using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class RangeBorderList
{
    List<RangeBorderGroup> groups;

    public RangeBorderList() {
        groups = new List<RangeBorderGroup>();
    }

    public void Add(int aX, int aY, int bX, int bY) {
        // If a group exists
        if (groups.Count > 0)
        {
            //For each group
            //  For each node 
            //      If both Nodes exist in the group then simply link them and close the group
            //          Else If no group contains that Node (X, Y) already then create a new group.
            //          Else Add that Node's pairing to the group.

            int contained = -1;
            bool combine = false;
            int i = 0;
            foreach(RangeBorderGroup group in groups.ToList())
            {
                combine = false;
                if ((group.Contains(aX, aY) || group.Contains(bX, bY)) && contained != -1)
                {
                    //One or both of the points are already contained within group "contained"
                    //Join the two groups on the points they share
                    combine = true;
                    foreach (RangeBorderNode node in group.nodes) 
                    {
                        if (!groups[contained].Contains(node))
                        {
                            groups[contained].Add(node);
                        }
                        else 
                        {
                            //If it does contain that node, dont add it, but ensure that the neighbors on the "Contained" list link up to the one that would be added.
                            if (groups[contained].Get(node.X, node.Y).GetNeighbor(0) == null) 
                            {
                                if (node.GetNeighbor(0) != null)
                                {
                                    groups[contained].Get(node.X, node.Y).SetNeighbor(0, node.GetNeighbor(0));
                                }
                            }

                            if (groups[contained].Get(node.X, node.Y).GetNeighbor(1) == null)
                            {
                                if (node.GetNeighbor(1) != null)
                                {
                                    groups[contained].Get(node.X, node.Y).SetNeighbor(1, node.GetNeighbor(1));
                                }
                            }
                        }
                    }
                    groups.Remove(group);
                }

                if (!combine)
                {
                    if (group.Contains(aX, aY) && group.Contains(bX, bY))
                    {
                        contained = i;
                        RangeBorderNode A = group.Get(aX, aY);
                        RangeBorderNode B = group.Get(bX, bY);

                        A.SetNeighbor(0, B);
                        B.SetNeighbor(1, A);

                        group.Close();
                    }
                    else if (group.Contains(aX, aY) && !group.Contains(bX, bY))
                    {
                        // Add B and link to A's open neighbor
                        // Add B to the group
                        contained = i;
                        RangeBorderNode A = group.Get(aX, aY);
                        RangeBorderNode B = new RangeBorderNode(A.group, bX, bY);

                        A.SetNeighbor(0, B);
                        B.SetNeighbor(1, A);
                        group.Add(B);
                    }
                    else if (!group.Contains(aX, aY) && group.Contains(bX, bY))
                    {
                        // Add A and link to B's open neighbor
                        contained = i;
                        RangeBorderNode B = group.Get(bX, bY);
                        RangeBorderNode A = new RangeBorderNode(B.group, aX, aY);

                        B.SetNeighbor(0, A);
                        A.SetNeighbor(1, B);
                        group.Add(A);
                    }
                }
                i++;
            }

            if (contained == -1) {
                //If no groups contains any of the nodes add a new group.
                AddGroup(aX, aY, bX, bY);
            }
        }
        else {
            //If no groups exist, add a new group from the given nodes.
            AddGroup(aX, aY, bX, bY);
        }
    }

    private void AddGroup(int aX, int aY, int bX, int bY) {
        RangeBorderNode A = new RangeBorderNode(groups.Count + 1, aX, aY);
        RangeBorderNode B = new RangeBorderNode(groups.Count + 1, bX, bY);

        A.SetNeighbor(0, B);
        B.SetNeighbor(1, A);

        RangeBorderGroup newGroup = new RangeBorderGroup(groups.Count + 1);
        newGroup.Add(A);
        newGroup.Add(B);

        groups.Add(newGroup);
    }

    public List<List<Vector3>> GetPoints() {
        //For each group create a List<Vector3> and fill it with all its points
        //Add that list to the Greater list
        //Return the Greater List
        List<List<Vector3>> BorderPoints = new List<List<Vector3>>();

        foreach (RangeBorderGroup grp_N in groups) {
            if (grp_N.closed)
            {
                if (grp_N.Count > 0)
                {
                    List<Vector3> points_N = new List<Vector3>();
                    RangeBorderNode pointer = grp_N.nodes[0];
                    points_N.Add(new Vector3(pointer.X, 0, pointer.Y));
                    pointer = pointer.GetNeighbor(0);
                    while (pointer != null && pointer != grp_N.nodes[0])
                    {
                        points_N.Add(new Vector3(pointer.X, 0, pointer.Y));
                        pointer = pointer.GetNeighbor(0);
                    }
                    BorderPoints.Add(points_N);
                }
            }
        }
        return BorderPoints;
    }
}

public class RangeBorderGroup 
{
    public List<RangeBorderNode> nodes;
    public bool closed { get; private set; }
    public int group { get; private set; }
    public int Count { get { return nodes.Count; } }

    public RangeBorderGroup(int _grp) {
        nodes = new List<RangeBorderNode>();
        group = _grp;
        closed = false;
    }

    public void Add(RangeBorderNode _node)
    {
        nodes.Add(_node);
    }

    public bool Contains(RangeBorderNode _node)
    {
        return Contains(_node.X,_node.Y);
    }

    public bool Contains(int _X, int _Y) 
    {
        if (nodes.Count > 0)
        {
            foreach (RangeBorderNode node in nodes)
            {
                if (node.X == _X && node.Y == _Y)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public RangeBorderNode Get(int _X, int _Y) {
        if (nodes.Count > 0)
        {
            foreach (RangeBorderNode node in nodes)
            {
                if (node.X == _X && node.Y == _Y)
                {
                    return node;
                }
            }
        }
        return null;
    }

    public void Close() {
        closed = true;
    }
}

public class RangeBorderNode {
    RangeBorderNode[] neighbors; //Only two possible neighbors -> The Primary that is added with the node initially, and the secondary that is attached if the node already exists in the group
    public int group { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public RangeBorderNode(int _grp, int _x, int _y) {
        neighbors = new RangeBorderNode[2];
        group = _grp;
        X = _x;
        Y = _y;
    }

    public void SetNeighbor(int _index, RangeBorderNode _neighbor) {
        if (_index < 2)
        {
            neighbors[_index] = _neighbor;
        }
        else {
            throw new IndexOutOfRangeException();
        }
    }

    public void SetOpenNeighbor(RangeBorderNode _neighbor)
    {
        for (int i = 0; i < 2; i++) {
            RangeBorderNode neighbor = neighbors[i];
            if (neighbor == null) {
                neighbors[i] = _neighbor;
                int j = i < 1 ? 1 : 0;
                _neighbor.SetNeighbor(j, this);
            }
        }
    }

    public RangeBorderNode GetNeighbor(int _n) 
    {
        return neighbors[_n];
    }
}
