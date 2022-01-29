using System;
using UnityEngine;

public abstract class IAction : MonoBehaviour
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

    public abstract string Name { get; }

    public abstract int Range { get; }
    public abstract TargetingType Targeting { get; }

    public virtual void PerformAction(GridManager gridManager, Vector3Int toCell)
    {
        Debug.Log($"Performing action {Name}");
    }
}
