using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridObject : MonoBehaviour
{
    [HideInInspector]
    public Vector3Int cellPosition;
    protected GridManager gridManager;


    public virtual GridObject Init(GridManager gridManager, Vector3Int cellPosition)
    {
        this.cellPosition = cellPosition;
        this.gridManager = gridManager;
        this.gridManager = gridManager;
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

    // These return true if the mode change was successful
    public virtual bool EnterMoveMode()
    {
        Debug.Log($"Entering move mode {name}");
        return true;
    }

    public virtual bool EnterActionMode()
    {
        Debug.Log($"Entering action mode {name}");
        return true;
    }

    public virtual void Move(Vector3Int toCell)
    {
        Debug.Log($"Move {name}");
        gridManager.SetPosition(cellPosition, toCell);
    }

    public virtual void PerformAction(Vector3Int toCell)
    {
        Debug.Log($"Performing action {name}");
    }

    public virtual void StartTurn()
    {
        Debug.Log($"Start Turn {name}");
    }

    public virtual void EndTurn()
    {
        Debug.Log($"End Turn {name}");
    }

    /// <returns>True if the click performed some action</returns>
    public virtual bool ClickCell(Vector3Int cell)
    {
        Debug.Log($"Click {name}");
        return false;
    }
}