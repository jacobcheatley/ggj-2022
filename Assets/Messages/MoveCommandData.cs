using UnityEngine;
using Newtonsoft.Json;


public class MoveCommandData : CommandData
{
    public int fromX;
    public int fromY;
    public int toX;
    public int toY;
    public MoveCommandData()
    {
        type = "move";
    }

    public MoveCommandData(int fromX, int fromY, int toX, int toY) : this()
    {
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
    }

    public override ICommand ToCommand()
    {
        return new MoveCommand(this);
    }

    [JsonIgnore]
    public Vector3Int FromCell => new Vector3Int(fromX, fromY, 0);
    [JsonIgnore]
    public Vector3Int ToCell => new Vector3Int(toX, toY, 0);
}