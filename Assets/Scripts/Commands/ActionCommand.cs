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

    public ActionCommand(SerializedActionCommand comm)
    {
        fromCell = comm.FromCell;
        toCell = comm.ToCell;
        actionId = comm.actionId;
    }

    public SerializedCommand Serialize()
    {
        return new SerializedActionCommand(fromCell.x, fromCell.y, toCell.x, toCell.y, actionId);
    }

    public void Execute()
    {
        Debug.Log("Doing an action");
        GridManager.instance.PerformAction(fromCell, toCell, actionId);
    }

    public void Undo()
    {
    }
}
