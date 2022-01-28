using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Message
{
    public string action;

    public Type AppropriateType()
    {
        switch (action)
        {
            case "code":
                return typeof(CodeMessage);
            case "message":
                return typeof(MessageMessage);
            //case "join":
            //    return typeof(JoinMessage);
            //case "create":
            //    return typeof(CreateMessage);
            case "connect":
                return typeof(ConnectMessage);
            case "disconnect":
                return typeof(DisconnectMessage);
            case "turn":
                return typeof(TurnMessage);
            case "error":
                return typeof(ErrorMessage);
            default:
                Debug.LogError($"Unhandled message type {action}");
                return typeof(Message);
        }
    }
}

public class CodeMessage : Message
{
    public string code;
}

public class MessageMessage : Message
{
    public string message;
}

public class ConnectMessage : Message
{
    public string name;
}

public class DisconnectMessage : Message
{
    public string name;
}

public class TurnMessage : Message
{
    //public List<ICommand> commands;
    public string turn;
}

public class ErrorMessage : Message
{
    public string type;
}
