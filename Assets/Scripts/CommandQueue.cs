using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandQueue : MonoBehaviour
{
    private List<ICommand> commands = new List<ICommand>();

    public void Submit(ICommand command)
    {
        command.Execute();
        commands.Add(command);
        Debug.Log($"Turn has {commands.Count} commands in it");
    }

    public void EndTurn()
    {
        commands.Clear();
        Debug.Log($"Ended turn with {commands.Count} commands");
    }
}
