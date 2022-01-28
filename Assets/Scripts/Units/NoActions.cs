using System.Collections;
using UnityEngine;

public class NoActions : IUnitActions
{
    protected override int[] ranges { get { return new int[] { }; } }

    public override void PerformAction(GridManager gridManager, Vector3Int toCell, int actionId)
    {
        base.PerformAction(gridManager, toCell, actionId);
    }
}
