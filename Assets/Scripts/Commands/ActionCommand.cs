using UnityEngine;

public class ActionCommand : ICommand
{
    private Vector3Int fromCell;
    private Vector3Int toCell;
    private int actionId;

    public ActionCommand(Vector3Int fromCell, Vector3Int toCell, int actionId)
    {
        this.fromCell = fromCell;
        this.toCell = toCell;
        this.actionId = actionId;
    }

    public void Execute()
    {
        GridManager.instance.PerformAction(fromCell, toCell, actionId);
    }

    public void Undo()
    {
    }
}
