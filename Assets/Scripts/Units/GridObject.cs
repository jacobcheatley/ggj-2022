using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridObject : MonoBehaviour
{
    [Flags]
    public enum Owner
    {
        Mine = 1,
        Theirs = 2,
        Terrain = 4,
        Unclaimed = 8,
    }

    [SerializeField]
    public Owner owner { get; private set; }

    [HideInInspector]
    public Vector3Int cellPosition;
    protected GridManager gridManager;
    protected CommandQueue commands;

    public virtual GridObject Init(
        GridManager gridManager,
        CommandQueue commands,
        Vector3Int cellPosition,
        Owner owner
        )
    {
        this.cellPosition = cellPosition;
        this.gridManager = gridManager;
        this.commands = commands;
        this.owner = owner;
        return this;
    }

    public virtual void Hover()
    {
        Debug.Log($"Hover {name}");
    }
    public virtual void Unhover()
    {
        Debug.Log($"Unhover {name}");
    }

    public virtual void Select()
    {
        Debug.Log($"Select {name}");
    }

    public virtual void Deselect()
    {
        Debug.Log($"Deselect {name}");
    }

    /// <returns>True if the mode change was successful. If false, no changes were applied.</returns>
    public virtual bool EnterMoveMode()
    {
        Debug.Log($"Entering move mode {name}");
        return true;
    }

    public virtual void SubmitMoveCommand(Vector3Int toCell)
    {
        Debug.Log($"Move {name}");
        commands.Submit(new MoveCommand(cellPosition, toCell));
    }

    public virtual bool HasAction(int actionId)
    {
        return false;
    }

    /// <returns>True if the mode change was successful. If false, no changes were applied.</returns>
    public virtual bool EnterActionMode(int actionId)
    {
        Debug.Log($"Entering action mode {actionId} {name}");
        return true;
    }

    public virtual void SubmitActionCommand(Vector3Int toCell, int actionId)
    {
        commands.Submit(new ActionCommand(cellPosition, toCell, actionId));
    }

    public virtual void PerformAction(Vector3Int toCell, int actionId)
    {
        Debug.Log($"Perform action {actionId}");
    }

    public virtual void StartTurn()
    {
        Debug.Log($"Start Turn {name}");
    }

    public virtual void EndTurn()
    {
        Debug.Log($"End Turn {name}");
    }

    public virtual bool HasDoneEverythingThisTurn()
    {
        return true;
    }

    /// <returns>True if the click performed some action</returns>
    public virtual bool ClickCell(Vector3Int cell)
    {
        Debug.Log($"Click {name}");
        return false;
    }

    public virtual void ApplyDamage(float amount)
    {
        Debug.Log($"Ouch. {name}");
    }

    public virtual void Destroy()
    {
        Debug.Log($"Big ouch. {name}");
        gridManager.RemoveObject(cellPosition);
    }
}
