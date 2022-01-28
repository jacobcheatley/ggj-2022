using UnityEngine;

public class MoveCommand : ICommand
{
    private Vector3Int fromCell;
    private Vector3Int toCell;

    public MoveCommand(Vector3Int fromCell, Vector3Int toCell)
    {
        this.fromCell = fromCell;
        this.toCell = toCell;
    }

    public void Execute()
    {
        GridManager.instance.Move(fromCell, toCell);
    }

    public void Undo()
    {
        GridManager.instance.SetPosition(toCell, fromCell);
    }
}
