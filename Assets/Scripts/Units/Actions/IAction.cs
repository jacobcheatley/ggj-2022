using System;
using UnityEditor;
using UnityEngine;

public abstract class IAction : ScriptableObject
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

    public string actionName = "Default Action Name";
    public string description = "Default Action Description";
    public Sprite icon;
    public int range = 1;
    public int value = 1;
    public Sprite valueIcon;
    [EnumFlags]
    public TargetingType targeting = TargetingType.Enemy;

    public virtual void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell)
    {
        Debug.Log($"Performing action {actionName}");
    }
}