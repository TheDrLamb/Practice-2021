using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public Unit unit;
    public UnitUIController uiController;

    public float unitMoveSpeed = 5.0f;
    Vector3 unitMapOffset = new Vector3(0.5f, 0.0f, 0.5f);

    public EventHandler UnitMoving;
    public EventHandler UnitStopped;

    public void SetTile(GridTile _tile) {
        if (unit.currentTile != null) unit.currentTile.occupied = false;
        unit.currentTile = _tile;
        unit.currentTile.occupied = true;
    }

    public void Move(List<PathNode> _path) {
        //Call Ienumerator to animate the movement along the path
        StopAllCoroutines();
        OnUnitMoving(new EventArgs());
        StartCoroutine(MoveAlongPath(_path));
    }

    IEnumerator MoveAlongPath(List<PathNode> _path) {
        //Move the character along the path, and set their current tile to target
        float moveSpeed;
        for (int i = 1; i < _path.Count; i++)
        {
            Vector3 a = new Vector3(_path[i - 1].X, 0, _path[i - 1].Y) + unitMapOffset;
            Vector3 b = new Vector3(_path[i].X, 0, _path[i].Y) + unitMapOffset;

            moveSpeed = unitMoveSpeed / Vector3.Distance(a, b); //Adjusts speed for moving at a diagonal

            for (float t = 0; t <= 1; t += Time.deltaTime * moveSpeed)
            {
                transform.localPosition = Vector3.Lerp(a, b, t);
                yield return null;
            }

            SetTile(_path[i].tile);
        }
        OnUnitStopped(new EventArgs());
    }

    public virtual void OnUnitMoving(EventArgs e) 
    {
        EventHandler handler = UnitMoving;
        handler?.Invoke(this, e);
    }

    public virtual void OnUnitStopped(EventArgs e)
    {
        EventHandler handler = UnitStopped;
        handler?.Invoke(this, e);
    }
}
