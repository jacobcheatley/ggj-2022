﻿using UnityEngine;

public class MoveCommand : ICommand
{
    private Vector3Int fromCell;
    private Vector3Int toCell;

    public MoveCommand(Vector3Int fromCell, Vector3Int toCell)
    {
        this.fromCell = fromCell;
        this.toCell = toCell;
    }

    public MoveCommand(SerializedMoveCommand comm)
    {
        fromCell = comm.FromCell;
        toCell = comm.ToCell;
    }

    public SerializedCommand Serialize()
    {
        return new SerializedMoveCommand(fromCell.x, fromCell.y, toCell.x, toCell.y);
    }

    public void Execute()
    {
        Debug.Log("Doing a movement");
        GridManager.instance.SetPosition(fromCell, toCell);
    }

    public void Undo()
    {
        // GridManager.instance.SetPosition(toCell, fromCell);
    }
}
