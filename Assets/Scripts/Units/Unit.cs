using System.Collections.Generic;
using UnityEngine;
public class Unit : GridObject
{
    [SerializeField]
    private int speed = 3;

    private float movedDistanceThisRound = 0;
    private List<Vector3Int> moveableCells;

    private bool hasMoved;
    private bool hasDoneAction;

    public override GridObject Init(GridManager gridManager, Vector3Int cellPosition)
    {
        moveableCells = gridManager.WithinCells(cellPosition, speed - movedDistanceThisRound);
        return base.Init(gridManager, cellPosition);
    }

    public override void Select()
    {
        base.Select();
        if (!hasMoved)
        {
            moveableCells = gridManager.WithinCells(cellPosition, speed - movedDistanceThisRound);
        }
        gridManager.HighlightCells(moveableCells, new Color(0, 1, 0, 0.25f));
    }

    public override bool ClickCell(Vector3Int cell)
    {
        // TODO: Logic for attacks if clicking on enemy... etc
        // Probably needs recalcuation or rethinking
        if (moveableCells.Contains(cell))
        {
            Move(cell);
            Deselect();
            // TODO: Fancy multi-step movement
            //movedDistanceThisRound += 1f;
            //gridManager.SelectCell(cell);
            return false;
        }
        else
        {
            return true;
        }
    }

    public override void Deselect()
    {
        base.Deselect();
        gridManager.ClearOverlay();
        gridManager.DeselectCurrentItem();
    }

    public override void Move(Vector3Int toCell)
    {
        if (!hasMoved)
        {
            base.Move(toCell);
            moveableCells.Clear();
        }
        hasMoved = true;
    }

    public override void StartTurn()
    {
        hasMoved = false;
        hasDoneAction = false;
        movedDistanceThisRound = 0;
    }

    public override void EndTurn()
    {

    }
}