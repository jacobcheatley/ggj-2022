using UnityEngine;
using Newtonsoft.Json;

public class ActionCommandData : CommandData
{
    public int fromX;
    public int fromY;
    public int toX;
    public int toY;
    public int actionId;

    public ActionCommandData()
    {
        type = "action";
    }

    public ActionCommandData(int fromX, int fromY, int toX, int toY, int actionId) : this()
    {
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
        this.actionId = actionId;
    }

    public override ICommand ToCommand()
    {
        return new ActionCommand(this);
    }

    [JsonIgnore]
    public Vector3Int FromCell => new Vector3Int(fromX, fromY, 0);
    [JsonIgnore]
    public Vector3Int ToCell => new Vector3Int(toX, toY, 0);
}
