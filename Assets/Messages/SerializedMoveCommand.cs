using System.Collections;
using UnityEngine;


public class SerializedMoveCommand : SerializedCommand
{
    public int fromX;
    public int fromY;
    public int toX;
    public int toY;

    public SerializedMoveCommand(int fromX, int fromY, int toX, int toY)
    {
        type = "move";
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
    }

    public Vector3Int FromCell => new Vector3Int(fromX, fromY, 0);
    public Vector3Int ToCell => new Vector3Int(toX, toY, 0);
}