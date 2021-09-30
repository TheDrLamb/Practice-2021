using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class GridTile
{
    public int ID, X, Y;
    public GridMap Map;
    public GridTile(int _ID, int _X, int _Y, GridMap _Map) {
        ID = _ID;
        X = _X;
        Y = _Y;
        Map = _Map;
        occupied = false;
    }

    public bool occupied;
}

public class GridMap
{
    public int size;
    public GridTile[,] tiles;

    public GridMap(int _size) {
        size = _size;
        tiles = new GridTile[_size, _size];

        GenerateMap();
    }

    public void GenerateMap() {
        for (int i = 0, n = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++, n++)
            {
                tiles[i, j] = new GridTile(n, i, j, this);
            }
        }
    }

    public int GetTileID(int _X, int _Y) {
        return tiles[_X, _Y].ID;
    }

    public GridTile GetTile(int _X, int _Y) {
        return tiles[_X, _Y];
    }
}

public class Unit
{
    public GridTile currentTile;
    public int maxAP, currentAP;
    public int maxHP, currentHP;
    public Unit() { 
        //Empty Constructor
    }

    public Unit(int _maxAP, int _maxHP) {
        maxAP = currentAP = _maxAP;
        maxHP = currentHP = _maxHP;
    }
}

public class PathNode {
    public int X {
        get
        {
            return tile.X;
        }
    }

    public int Y
    {
        get
        {
            return tile.Y;
        }
    }

    public Vector3 position {
        get {
            return new Vector3(tile.X, tile.Y, 0);
        }
    }

    public int g, h;
    public int f {
        get 
        {
            return g + h;
        }
    }
    public GridTile tile;
    public PathNode parent;

    public PathNode(GridTile _tile) {
        tile = _tile;
    }
}
