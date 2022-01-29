using System;
using UnityEngine;


public class RangedAttackAction : IAction
{
    [SerializeField]
    private int damage = 1;

    public override string Name => "Melee Attack";

    public override int Range => 3;
    public override TargetingType Targeting => TargetingType.Ally | TargetingType.Enemy;

    public override void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell)
    {
        base.PerformAction(gridManager, fromCell, toCell);

        GridObject other = gridManager.GetAtPosition(toCell);
        other?.ApplyDamage(damage);
    }
}
