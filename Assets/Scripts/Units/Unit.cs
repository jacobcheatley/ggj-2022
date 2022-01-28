using System.Collections.Generic;
using UnityEngine;
public class Unit : GridObject
{
    [SerializeField]
    private int speed = 3;

    private float movedDistanceThisRound = 0;
    private List<Vector3Int> moveableCells;

    public override GridObject Init(GridManager gridManager, Vector3Int cellPosition)
    {
        moveableCells = gridManager.WithinCells(cellPosition, speed - movedDistanceThisRound);
        return base.Init(gridManager, cellPosition);
    }

    public override void Select()
    {
        base.Select();
        moveableCells = gridManager.WithinCells(cellPosition, speed - movedDistanceThisRound);
        gridManager.HighlightCells(moveableCells, new Color(0, 1, 0, 0.25f));
    }

    public override bool ClickCell(Vector3Int cell)
    {
        // TODO: Logic for attacks if clicking on enemy... etc
        // Probably needs recalcuation or rethinking
        if (moveableCells.Contains(cell))
        {
            Move(cell); // TODO: Move command
            //movedDistanceThisRound += 1f;
            //gridManager.SelectCell(cell);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Deselect()
    {
        base.Deselect();
        gridManager.ClearOverlay();
    }

    public override void Move(Vector3Int toCell)
    {
        base.Move(toCell);
    }
    public override void Turn()
    {
        movedDistanceThisRound = 0;
    }
}