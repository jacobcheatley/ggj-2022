using System;
using UnityEngine;


public class FlyAction : IAction
{
    public override string Name => "Fly";

    public override int Range => 5;
    public override TargetingType Targeting => TargetingType.Empty;

    public override void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell)
    {
        base.PerformAction(gridManager, fromCell, toCell);

        CommandQueue.instance.Submit(new MoveCommand(fromCell, toCell));
    }
}
