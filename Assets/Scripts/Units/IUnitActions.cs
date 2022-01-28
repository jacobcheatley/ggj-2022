using System.Collections;
using UnityEngine;

public abstract class IUnitActions : MonoBehaviour
{
    protected abstract int[] ranges { get; }

    public bool HasAnyActions { get { return ranges.Length > 0; } }

    public bool HasAction(int actionId)
    {
        return actionId < ranges.Length;
    }

    public int GetActionRange(int actionId)
    {
        return ranges[actionId];
    }

    public virtual void PerformAction(GridManager gridManager, Vector3Int toCell, int actionId)
    {
        Debug.Log($"Performing action {actionId}");
    }
}
