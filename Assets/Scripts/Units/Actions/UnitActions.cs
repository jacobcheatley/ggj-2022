using System;
using UnityEngine;

public class UnitActions : MonoBehaviour
{
    protected IAction[] actions;

    public void Init()
    {
        actions = GetComponents<IAction>();
    }

    public bool HasAnyActions { get { return actions.Length > 0; } }

    public bool HasAction(int actionId)
    {
        return actionId < actions.Length;
    }

    public int GetActionRange(int actionId)
    {
        return actions[actionId].Range;
    }

    public IAction.TargetingType GetTargetingType(int actionId)
    {
        return actions[actionId].Targeting;
    }

    public void PerformAction(GridManager gridManager, Vector3Int toCell, int actionId)
    {
        actions[actionId].PerformAction(gridManager, toCell);
    }
}
