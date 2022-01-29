using Newtonsoft.Json;
using System;
using UnityEngine;

public class SerializedCommand
{
    public string type;

    public Type GetAppropriateType()
    {
        switch (type)
        {
            case "action":
                return typeof(ActionCommand);
            case "move":
                return typeof(MoveCommand);
            default:
                Debug.LogError($"Unhandled message type {type}");
                return typeof(SerializedCommand);
        }
    }

    public static ICommand Deserialize(string data)
    {
        var getTyper = JsonConvert.DeserializeObject<SerializedCommand>(data);
        Type properType = getTyper.GetAppropriateType();
        dynamic t = JsonConvert.DeserializeObject(data, properType);
        switch (t)
        {
            case SerializedActionCommand comm:
                return new ActionCommand(comm);
            case SerializedMoveCommand comm:
                return new MoveCommand(comm);
            case SerializedCommand _:
            default:
                Debug.LogError($"Unsupported command type {t}");
                return null;
        }
    }
}
