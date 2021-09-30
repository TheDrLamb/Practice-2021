using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridMapRangefinder
{
    GridMap map;
    GridMapPathfinder pathfinder;

    public GridMapRangefinder(GridMap _map)
    {
        map = _map;
    }

    //[NOTE] -> This represents only the "Simple" range of the Character, i.e. all of the possible moves that they could take, it does not account for movement costs 
    public List<GridTile> GetSimpleRange(Unit _unit)
    {
        GridTile origin = _unit.currentTile;
        int movementRange = _unit.currentAP;

        List<GridTile> range = new List<GridTile>();
        range.Add(origin);

        for (int i = -movementRange; i <= movementRange; i++)
        {
            for (int j = -movementRange; j <= movementRange; j++)
            {
                int x, y;
                x = origin.X + i;
                y = origin.Y + j;
                if (x >= 0 && y >= 0 && x < map.size && y < map.size)
                {
                    GridTile pointer = map.GetTile(x, y);
                    if (!pointer.occupied) range.Add(pointer);
                }
            }
        }
        return range;
    }

    public List<GridTile> GetRange(Unit _unit) {
        if (pathfinder == null) pathfinder = new GridMapPathfinder(map);
        List<GridTile> simpleRange = GetSimpleRange(_unit);
        simpleRange.Remove(_unit.currentTile);
        foreach (GridTile tile in simpleRange.ToList()) {
            if (pathfinder.FindPath(_unit.currentTile, tile).Count - 1 > _unit.currentAP) {
                simpleRange.Remove(tile);
            }
        }

        return simpleRange;
    }
}