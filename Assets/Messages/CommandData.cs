using Newtonsoft.Json;
using System;
using UnityEngine;

public class CommandData
{
    public string type;

    public virtual ICommand ToCommand()
    {
        throw new NotImplementedException();
    }
}
