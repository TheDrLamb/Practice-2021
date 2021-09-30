using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RangeController : MonoBehaviour
{
    public GridMapController gridMap;

    GridMapRangefinder rangefinder;

    List<GridTile> range;
    
    public GameObject rangeTile;

    List<GameObject> rangeTiles;

    public bool rangeSet = false;

    private void Start()
    {
        range = new List<GridTile>();
        rangeTiles = new List<GameObject>();
    }

    public void SetRange(Unit _unit) {
        //Clear all the old range tiles if any exist
        ClearRange();
        //Get the Range from Rangefinder
        if (rangefinder == null) rangefinder = new GridMapRangefinder(gridMap.map);
        range = rangefinder.GetRange(_unit);
        //Generate a mesh for that range.
        VisualizeRange();
    }

    public void ClearRange() {
        range.Clear();
        foreach (GameObject tile in rangeTiles) {
            Destroy(tile);
        }
        rangeTiles.Clear();
        rangeSet = false;
    }

    void VisualizeRange()
    {
        foreach (GridTile gTile in range) 
        {
            GameObject newRangeTile = Instantiate(rangeTile, this.transform);
            newRangeTile.transform.localPosition = new Vector3(gTile.X, 0, gTile.Y);
            rangeTiles.Add(newRangeTile);
        }
        rangeSet = true;
    }
}
