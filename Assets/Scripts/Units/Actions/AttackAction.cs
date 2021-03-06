using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/AttackAction")]
public class AttackAction : IAction
{
    [SerializeField]
    private bool drain = false;

    public override void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell)
    {
        base.PerformAction(gridManager, fromCell, toCell);

        GridObject other = gridManager.GetAtPosition(toCell);
        other?.ApplyDamage(value);
        if (drain)
        {
            GridObject self = gridManager.GetAtPosition(fromCell);
            self?.ApplyDamage(-value);
        }
    }
}
