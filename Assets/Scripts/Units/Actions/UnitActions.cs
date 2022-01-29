using UnityEngine;

public class UnitActions : MonoBehaviour
{
    [HideInInspector]
    public IAction[] actions { get; protected set; }

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
        return actions[actionId].range;
    }

    public IAction.TargetingType GetTargetingType(int actionId)
    {
        return actions[actionId].targeting;
    }

    public void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell, int actionId)
    {
        actions[actionId].PerformAction(gridManager, fromCell, toCell);
    }
}
