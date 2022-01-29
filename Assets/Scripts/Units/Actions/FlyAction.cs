using System;
using UnityEngine;


public class FlyAction : IAction
{
    public override void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell)
    {
        base.PerformAction(gridManager, fromCell, toCell);

        GridManager.instance.SetPosition(fromCell, toCell);
    }
}
