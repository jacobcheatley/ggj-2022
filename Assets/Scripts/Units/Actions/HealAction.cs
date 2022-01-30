using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/HealAction")]
public class HealAction : IAction
{
    public override void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell)
    {
        base.PerformAction(gridManager, fromCell, toCell);

        GridObject other = gridManager.GetAtPosition(toCell);
        other?.ApplyDamage(value);
    }
}
