using System.Collections.Generic;
using UnityEngine;
public class Unit : GridObject
{
    [SerializeField]
    private int speed = 3;
    [SerializeField]
    private int actionRange = 1;

    [SerializeField]
    private Color movementHighlightColor = new Color(0, 1, 0, 0.25f);
    [SerializeField]
    private Color actionHighlightColor = new Color(1, 0, 0, 0.25f);

    private float movedDistanceThisRound = 0;
    private List<Vector3Int> interactiveCells = new List<Vector3Int>();

    private bool hasMoved;
    private bool hasDoneAction;

    enum SelectionMode
    {
        None,
        Movement,
        Action
    }

    private SelectionMode selectionMode = SelectionMode.None;

    public override GridObject Init(GridManager gridManager, Vector3Int cellPosition)
    {
        //moveableCells = gridManager.WithinEmptyCells(cellPosition, speed - movedDistanceThisRound);

        return base.Init(gridManager, cellPosition);
    }

    public override void Select()
    {
        base.Select();
        if (!hasMoved)
        {
            EnterMoveMode();
        }
        else if (!hasDoneAction)
        {
            EnterActionMode();
        }
    }

    private void UpdateInteractiveCells(List<Vector3Int> cells, Color color)
    {
        interactiveCells = cells;
        gridManager.HighlightCells(interactiveCells, color);
    }

    private void ClearInteractiveCells()
    {
        interactiveCells.Clear();
        gridManager.ClearOverlay();
    }

    public override bool EnterMoveMode()
    {
        base.EnterMoveMode();

        if (!hasMoved)
        {
            selectionMode = SelectionMode.Movement;

            ClearInteractiveCells();
            UpdateInteractiveCells(gridManager.WithinMoveableCells(cellPosition, speed - movedDistanceThisRound), movementHighlightColor);

            return true;
        }
        else
        {
            ClearInteractiveCells();
            return false;
        }
    }

    public override bool EnterActionMode()
    {
        base.EnterActionMode();

        if (!hasDoneAction)
        {
            selectionMode = SelectionMode.Action;

            ClearInteractiveCells();
            UpdateInteractiveCells(gridManager.WithinActionableCells(cellPosition, actionRange), actionHighlightColor);

            return true;
        }
        else
        {
            ClearInteractiveCells();
            return false;
        }
    }

    public override bool ClickCell(Vector3Int cell)
    {
        base.ClickCell(cell);

        // TODO: Logic for attacks if clicking on enemy... etc
        // Probably needs recalcuation or rethinking
        switch (selectionMode)
        {
            case SelectionMode.None:
                return false;
            case SelectionMode.Movement:
                if (interactiveCells.Contains(cell))
                {
                    Move(cell);
                    return true;
                }
                return false;
            case SelectionMode.Action:
                if (interactiveCells.Contains(cell))
                {
                    PerformAction(cell);
                    return true;
                }
                return false;
            default:
                return false;
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
            // TODO: Fancy multi-step movement
            //movedDistanceThisRound += 1f;
            //gridManager.SelectCell(cell);
            base.Move(toCell);
            ClearInteractiveCells();
            hasMoved = true;

            if (!hasDoneAction)
            {
                EnterActionMode();
            }
            else
            {
                Deselect();
            }
        }
    }

    public override void PerformAction(Vector3Int target)
    {
        if (!hasDoneAction)
        {
            base.PerformAction(target);
            ClearInteractiveCells();
            hasDoneAction = true;

            if (!hasMoved)
            {
                EnterMoveMode();
            }
            else
            {
                Deselect();
            }
        }
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