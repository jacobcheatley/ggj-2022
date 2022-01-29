using System.Collections.Generic;
using UnityEngine;
public class Unit : GridObject
{
    [Header("Unit Info")]
    public string unitName;
    public string description;

    [Header("Unit Stats - Daytime")]
    public int dayHealth = 5;
    public int daySpeed = 2;
    [SerializeField]
    private UnitActions dayActions;
    [SerializeField]
    private Sprite daySprite;

    [Header("Unit Stats - Nighttime")]
    public int nightHealth = 5;
    public int nightSpeed = 3;
    [SerializeField]
    private UnitActions nightActions;
    [SerializeField]
    private Sprite nightSpright;

    [Header("Colours")]
    [SerializeField]
    private Color movementHighlightColor = new Color(0, 1, 0, 0.25f);
    [SerializeField]
    private Color actionHighlightColor = new Color(1, 0, 0, 0.25f);
    [SerializeField]
    private Color actionRangeHighlightColor = new Color(1, 0, 0, 0.1f);
    [SerializeField]
    private Color mineColor = new Color(0, 0.5f, 1, 0.5f);
    [SerializeField]
    private Color theirsColor = new Color(1, 0.5f, 0, 0.5f);

    [Header("SpriteRenderers")]
    [SerializeField]
    private SpriteRenderer character;
    [SerializeField]
    private SpriteRenderer shadow;

    private float movedDistanceThisRound = 0;
    private List<Vector3Int> interactiveCells = new List<Vector3Int>();

    private bool isDaytime;
    private bool hasMoved;
    private bool hasDoneAction;

    private int maxHealth { get { return isDaytime ? dayHealth : nightHealth; } }
    private int speed { get { return isDaytime ? dayHealth : nightHealth; } }
    private UnitActions actions { get { return isDaytime ? dayActions : nightActions; } }

    // TODO: Change current health between day and night
    private int currentHealth;

    enum SelectionMode
    {
        None,
        Movement,
        Action
    }

    private SelectionMode selectionMode = SelectionMode.None;
    private int selectedAction = 0;

    private SelectionMode previousSelectionMode = SelectionMode.None;
    private int previousSelectedAction = 0;

    public override GridObject Init(GridManager gridManager, CommandQueue commands, Vector3Int cellPosition, Owner owner)
    {
        currentHealth = dayHealth;
        if (owner == Owner.Mine)
        {
            shadow.color = mineColor;
        }
        else
        {
            shadow.color = theirsColor;
        }
        return base.Init(gridManager, commands, cellPosition, owner);
    }

    public override void Select()
    {
        base.Select();
        SelectedUnitUI.instance.DisplayUnit(this);
        if (!hasMoved)
        {
            EnterMoveMode();
        }
        else if (!hasDoneAction)
        {
            EnterActionMode(0);
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

    private void RecordCurrentMode()
    {
        previousSelectionMode = selectionMode;
        previousSelectedAction = selectedAction;
    }

    private void RestoreMode()
    {
        switch (previousSelectionMode)
        {
            case SelectionMode.None:
                selectionMode = previousSelectionMode;
                break;
            case SelectionMode.Movement:
                EnterMoveMode();
                break;
            case SelectionMode.Action:
                EnterActionMode(previousSelectedAction);
                break;
            default:
                selectionMode = SelectionMode.None;
                break;
        }
    }

    public override bool EnterMoveMode()
    {
        RecordCurrentMode();
        base.EnterMoveMode();

        if (!hasMoved)
        {
            selectionMode = SelectionMode.Movement;

            ClearInteractiveCells();
            UpdateInteractiveCells(
                gridManager.WithinCells(
                    cellPosition,
                    speed - movedDistanceThisRound,
                    (manager, x, y, pos) =>
                    {
                        if (x == 0 && y == 0)
                        {
                            return false;
                        }
                        return manager.PositionIsEmpty(pos);
                    }
                ),
                movementHighlightColor
            );

            return true;
        }
        else
        {
            ClearInteractiveCells();
            RestoreMode();
            return false;
        }
    }

    public override bool HasAction(int actionId)
    {
        return actions.HasAction(actionId);
    }

    public override bool EnterActionMode(int actionId)
    {
        RecordCurrentMode();
        base.EnterActionMode(actionId);

        if (!actions.HasAction(actionId))
        {
            ClearInteractiveCells();
            Debug.Log("Don't have that action");
            RestoreMode();
            return false;
        }

        if (!hasDoneAction)
        {
            selectionMode = SelectionMode.Action;
            selectedAction = actionId;

            ClearInteractiveCells();
            // First apply the range colour
            UpdateInteractiveCells(
                gridManager.WithinCells(
                    cellPosition,
                    actions.GetActionRange(actionId),
                    (manager, x, y, pos) =>
                    {
                        if (x == 0 && y == 0)
                        {
                            return false;
                        }
                        return true;
                    }
                ),
                actionRangeHighlightColor
            );
            // Override targetable tiles with the proper highlight colour
            UpdateInteractiveCells(
                gridManager.WithinCells(
                    cellPosition,
                    actions.GetActionRange(actionId),
                    (manager, x, y, pos) =>
                    {
                        if (x == 0 && y == 0)
                        {
                            return actions.GetTargetingType(actionId).HasFlag(IAction.TargetingType.Self);
                        }
                        if (manager.PositionIsEmpty(pos))
                        {
                            return actions.GetTargetingType(actionId).HasFlag(IAction.TargetingType.Empty);
                        }
                        var target = manager.GetAtPosition(pos);
                        switch (target.owner)
                        {
                            case Owner.Mine:
                                return actions.GetTargetingType(actionId).HasFlag(IAction.TargetingType.Ally);
                            case Owner.Theirs:
                                return actions.GetTargetingType(actionId).HasFlag(IAction.TargetingType.Enemy);
                            case Owner.Terrain:
                                return actions.GetTargetingType(actionId).HasFlag(IAction.TargetingType.Terrain);
                            case Owner.Unclaimed:
                                return actions.GetTargetingType(actionId).HasFlag(IAction.TargetingType.Unclaimed);
                            default:
                                Debug.Log($"Unknown targeting result for owner {target.owner}");
                                return true;
                        }
                    }
                ),
                actionHighlightColor
            );

            return true;
        }
        else
        {
            Debug.Log("Already done an action");
            ClearInteractiveCells();
            RestoreMode();
            return false;
        }
    }

    public override bool ClickCell(Vector3Int cell)
    {
        base.ClickCell(cell);

        switch (selectionMode)
        {
            case SelectionMode.None:
                return false;
            case SelectionMode.Movement:
                if (interactiveCells.Contains(cell))
                {
                    SubmitMoveCommand(cell);
                    return true;
                }
                return false;
            case SelectionMode.Action:
                if (interactiveCells.Contains(cell))
                {
                    SubmitActionCommand(cell, selectedAction);
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

    public override void SubmitMoveCommand(Vector3Int toCell)
    {
        if (!hasMoved)
        {
            // TODO: Fancy multi-step movement
            //movedDistanceThisRound += 1f;
            //gridManager.SelectCell(cell);
            base.SubmitMoveCommand(toCell);
            ClearInteractiveCells();
            hasMoved = true;

            if (!hasDoneAction)
            {
                EnterActionMode(0);
            }
            else
            {
                Deselect();
            }
        }
    }

    public override void SubmitActionCommand(Vector3Int target, int actionId)
    {
        if (!hasDoneAction)
        {
            base.SubmitActionCommand(target, actionId);
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

    public override void PerformAction(Vector3Int toCell, int actionId)
    {
        base.PerformAction(toCell, actionId);

        actions.PerformAction(gridManager, cellPosition, toCell, actionId);
    }

    public override void ApplyDamage(float amount)
    {
        base.ApplyDamage(amount);
        // All damage is lethal :)
        Destroy();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void StartTurn(GridManager.TimeOfDay time)
    {
        if (time == GridManager.TimeOfDay.Daytime)
        {
            isDaytime = true;
            character.sprite = daySprite;
        }
        else
        {
            isDaytime = false;
            character.sprite = nightSpright;
        }
        hasMoved = false;
        hasDoneAction = !actions.HasAnyActions;
        movedDistanceThisRound = 0;
    }

    public override void EndTurn()
    {

    }

    public override bool HasDoneEverythingThisTurn()
    {
        return hasMoved && hasDoneAction;
    }
}