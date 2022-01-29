using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandQueue : MonoBehaviour
{
    private List<ICommand> commands = new List<ICommand>();

    public static CommandQueue instance;

    private void Start()
    {
        instance = this;
    }

    public void Submit(ICommand command)
    {
        command.Execute();
        commands.Add(command);
        Debug.Log($"Turn has {commands.Count} commands in it");
    }

    public void EndTurn()
    {
        List<CommandData> serialized = new List<CommandData>();
        foreach (var item in commands)
        {
            serialized.Add(item.ToData());
        }

        var message = new TurnMessage
        {
            action = "turn",
            commands = serialized
        };

        NetworkManager.instance.SendMessage(message);

        Debug.Log($"Ended turn with {commands.Count} commands");
        commands.Clear();
    }
}
