using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

class MessageConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Message).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        var action = (string)jObject["action"];
        Message target;
        switch (action)
        {
            case "code":
                target = new CodeMessage();
                break;
            case "message":
                target = new MessageMessage();
                break;
            case "connect":
                target = new ConnectMessage();
                break;
            case "disconnect":
                target = new DisconnectMessage();
                break;
            case "turn":
                target = new TurnMessage();
                break;
            case "error":
                target = new ErrorMessage();
                break;
            default:
                target = new Message();
                break;
        }
        serializer.Populate(jObject.CreateReader(), target);
        return target;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

class CommandConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(CommandData).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);
        var type = (string)jObject["type"];
        CommandData target;
        switch (type)
        {
            case "move":
                target = new MoveCommandData();
                break;
            case "action":
                target = new ActionCommandData();
                break;
            default:
                target = new CommandData();
                break;
        }
        serializer.Populate(jObject.CreateReader(), target);
        return target;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}