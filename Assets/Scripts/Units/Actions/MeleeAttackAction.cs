using System;
using UnityEngine;


public class MeleeAttackAction : IAction
{
    [SerializeField]
    private int damage = 3;

    public override string Name => "Melee Attack";

    public override int Range => 1;
    public override TargetingType Targeting => TargetingType.Ally | TargetingType.Enemy | TargetingType.Empty | TargetingType.Terrain | TargetingType.Unclaimed;

    public override void PerformAction(GridManager gridManager, Vector3Int toCell)
    {
        base.PerformAction(gridManager, toCell);

        GridObject other = gridManager.GetAtPosition(toCell);
        other?.ApplyDamage(damage);
    }
}
