using System;
using UnityEngine;

public abstract class IUnitActions : MonoBehaviour
{
    [Flags]
    public enum TargetingType
    {
        Empty = 1,
        Terrain = 2,
        Self = 4,
        Ally = 8,
        Enemy = 16,
        Unclaimed = 32,
    }

    protected abstract int[] ranges { get; }
    protected abstract TargetingType[] targetingTypes { get; }

    public bool HasAnyActions { get { return ranges.Length > 0; } }

    public bool HasAction(int actionId)
    {
        return actionId < ranges.Length;
    }

    public int GetActionRange(int actionId)
    {
        return ranges[actionId];
    }

    public TargetingType GetTargetingType(int actionId)
    {
        return targetingTypes[actionId];
    }

    public virtual void PerformAction(GridManager gridManager, Vector3Int toCell, int actionId)
    {
        Debug.Log($"Performing action {actionId}");
    }
}
