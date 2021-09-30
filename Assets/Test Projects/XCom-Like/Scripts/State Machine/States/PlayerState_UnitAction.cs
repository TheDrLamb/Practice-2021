using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState_UnitAction : PlayerState
{
    GridMapPathfinder pathfinder;
    public PlayerState_UnitAction(PlayerController _player, PlayerStateMachine _statemachine) : base(_player, _statemachine)
    {
        //Selecting Unit Constructor
        pathfinder = new GridMapPathfinder(player.gridMap.map);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Current State: Unit Action");
        player.rangeController.SetRange(player.selectedUnit.unit);
    }

    public override void Update()
    {
        base.Update();

        VisualUpdate();
        InputUpdate();
    }

    protected override void InputUpdate()
    {
        base.InputUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            LayerMask map = LayerMask.GetMask("Map");

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, map))
            {
                GridTile hitTile = player.gridMap.GetTile(hit.point);
                //If there is a selected Unit and youre not pointing at it
                if (hitTile != player.selectedUnit.unit.currentTile)
                {
                    List<PathNode> path = pathfinder.FindPath(player.selectedUnit.unit.currentTile, hitTile);

                    //If the tile under the mouse is a valid move
                    if (path != null && path.Count - 1 <= player.selectedUnit.unit.currentAP)
                    {
                        //Move the unit
                        Debug.Log("Moving Unit");

                        player.selectedUnit.Move(path);
                        player.selectedUnit.unit.currentAP -= path.Count - 1;
                        player.selectedUnit.GetComponentInChildren<UnitUIController>().UpdateStats();
                    }
                }
            }
        }
    }

    protected override void VisualUpdate()
    {
        base.VisualUpdate();

        RaycastHit hit;
        LayerMask map = LayerMask.GetMask("Map");
        bool check = false;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, map))
        {
            GridTile hitTile = player.gridMap.GetTile(hit.point);
            //If there is a selected Unit and youre not pointing at it
            if (hitTile != player.selectedUnit.unit.currentTile)
            {
                List<PathNode> path = pathfinder.FindPath(player.selectedUnit.unit.currentTile, hitTile);

                //If the tile under the mouse is a valid move
                if (path != null && path.Count - 1 <= player.selectedUnit.unit.currentAP)
                {
                    check = true;
                    player.pathController.SetPath(path);
                }
            }
        }
        if (!check) player.pathController.Clear();
    }

    public override void Exit()
    {
        base.Exit();

        player.pathController.Clear();
        player.rangeController.ClearRange();
    }
}
