using System.Collections;
using UnityEngine;

public class SerializedActionCommand : SerializedCommand
{
    public int fromX;
    public int fromY;
    public int toX;
    public int toY;
    public int actionId;

    public SerializedActionCommand(int fromX, int fromY, int toX, int toY, int actionId)
    {
        type = "action";
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
        this.actionId = actionId;
    }

    public Vector3Int FromCell => new Vector3Int(fromX, fromY, 0);
    public Vector3Int ToCell => new Vector3Int(toX, toY, 0);
}
