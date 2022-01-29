using UnityEngine;

public class UnitActions : MonoBehaviour
{
    [SerializeField]
    private string label;
    public IAction[] actions;

    public void Init()
    {
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
