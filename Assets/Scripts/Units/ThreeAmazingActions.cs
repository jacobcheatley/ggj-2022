using System.Collections;
using UnityEngine;

public class ThreeAmazingActions : IUnitActions
{
    protected override int[] ranges { get { return new int[] { 1, 2, 3 }; } }
    protected override TargetingType[] targetingTypes
    {
        get
        {
            return new[]{
                TargetingType.Empty | TargetingType.Terrain | TargetingType.Ally | TargetingType.Enemy,
                TargetingType.Ally | TargetingType.Enemy,
                TargetingType.Empty | TargetingType.Terrain,
            };
        }
    }

    public override void PerformAction(GridManager gridManager, Vector3Int toCell, int actionId)
    {
        base.PerformAction(gridManager, toCell, actionId);
    }
}
