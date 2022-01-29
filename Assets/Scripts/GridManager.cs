using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [Header("Map Generation")]
    // TODO spawn different things, generate a more interesting map, probably make this a scriptable object
    [SerializeField]
    private GameObject testStartObject;

    [SerializeField]
    private Vector2Int[] team1SpawnPositions;
    [SerializeField]
    private Vector2Int[] team2SpawnPositions;

    // TODO: Abstracted square info which has a building layer and creature layer, rather than just game objects
    //private GridObject[,] gridObjects = new GridObject[X, Y];
    private Dictionary<Vector3Int, GridObject> gridObjects = new Dictionary<Vector3Int, GridObject>();
    private GridObject selectedObject;
    private GridObject hoveredObject;

    private Grid grid;
    [Header("Tiles")]
    //[SerializeField]
    //private Tilemap terrain;
    [SerializeField]
    private Tilemap overlaysTilemap;
    [SerializeField]
    private Tile selectTile;
    [SerializeField]
    private Tile blankTile;

    [Header("The other stuff")]
    [SerializeField]
    private CommandQueue commands;

    [Header("Audio")]
    [SerializeField]
    private AudioClip daySound;
    [SerializeField]
    private AudioClip nightSound;

    public enum Turn
    {
        Mine,
        Theirs
    }

    public enum TimeOfDay
    {
        Daytime,
        Nighttime
    }

    private Turn whoStarted;
    private Turn whoseTurn;

    private TimeOfDay timeOfDay;
    private int roundsInCurrentTimeOfDay = 0;

    public static GridManager instance;

    public void Init(Turn startingTurn)
    {
        instance = this;
        grid = GetComponent<Grid>();

        whoStarted = startingTurn;
        whoseTurn = startingTurn;
        timeOfDay = TimeOfDay.Daytime;
        SoundManager.instance.PlayClip(daySound);

        foreach (var spawnPosition in team1SpawnPositions)
        {
            var owner = startingTurn == Turn.Mine ? GridObject.Owner.Mine : GridObject.Owner.Theirs;
            var gridObject = SpawnGridObject(testStartObject, spawnPosition, owner);
            if (owner == GridObject.Owner.Mine)
            {
                gridObject.StartTurn(timeOfDay);
            }
        }

        foreach (var spawnPosition in team2SpawnPositions)
        {
            var owner = startingTurn == Turn.Theirs ? GridObject.Owner.Mine : GridObject.Owner.Theirs;
            var gridObject = SpawnGridObject(testStartObject, spawnPosition, owner, flipped: true);
            if (owner == GridObject.Owner.Mine)
            {
                gridObject.StartTurn(timeOfDay);
            }
        }

        NetworkManager.instance.OnTurnMessage += ReceiveTurn;
    }

    public GridObject SpawnGridObject(GameObject gridObjectPrefab, Vector2Int cellPosition, GridObject.Owner owner, bool flipped = false)
    {
        return SpawnGridObject(gridObjectPrefab, new Vector3Int(cellPosition.x, cellPosition.y, 0), owner, flipped);
    }

    public GridObject SpawnGridObject(GameObject gridObjectPrefab, Vector3Int cellPosition, GridObject.Owner owner, bool flipped = false)
    {
        gridObjects[cellPosition] = Instantiate(
            gridObjectPrefab,
            grid.CellToWorld(cellPosition),
            Quaternion.identity
            )
            .GetComponent<GridObject>()
        .Init(
            this,
            commands,
            cellPosition,
            owner,
            flipped
        );
        return gridObjects[cellPosition];
    }

    public bool PositionIsEmpty(Vector3Int pos)
    {
        return !gridObjects.ContainsKey(pos);
    }

    public GridObject GetAtPosition(Vector3Int pos)
    {
        GridObject result;
        if (gridObjects.TryGetValue(pos, out result))
        {
            return result;
        }
        return null;
    }

    public void SetPosition(Vector3Int fromCell, Vector3Int toCell)
    {
        Debug.Assert(!gridObjects.ContainsKey(toCell));
        GridObject movedObject = gridObjects[fromCell];
        movedObject.cellPosition = toCell;
        movedObject.transform.position = grid.CellToWorld(toCell);
        gridObjects[toCell] = movedObject;
        gridObjects.Remove(fromCell);
    }

    public void RemoveObject(Vector3Int atCell)
    {
        GridObject obj = GetAtPosition(atCell);
        Debug.Log($"Removing object {obj.name}");
        gridObjects.Remove(atCell);
        Destroy(obj.gameObject);
        hoveredObject = null;
    }

    public void PerformAction(Vector3Int fromCell, Vector3Int toCell, int actionId)
    {
        gridObjects[fromCell].PerformAction(toCell, actionId);
    }

    public List<Vector3Int> WithinCells(
        Vector3Int fromCell,
        float distance,
        Func<GridManager, int, int, Vector3Int, bool> shouldKeepItem
        )
    {
        // TODO: Probably need to Dijkstra's and figure out paths at some point - but not yet!
        var result = new List<Vector3Int>();
        float squaredDistance = distance * distance + 1f; // Extra 1 allows a cheeky little extra diagonal for free

        for (int x = (int)-distance; x <= (int)distance; x++)
        {
            for (int y = (int)-distance; y <= (int)distance; y++)
            {
                if (x * x + y * y <= squaredDistance)
                {
                    var targetCell = fromCell + Vector3Int.right * x + Vector3Int.up * y;
                    if (shouldKeepItem(this, x, y, targetCell))
                    {
                        result.Add(targetCell);
                    }
                }
            }
        }

        return result;
    }

    public void HighlightCells(List<Vector3Int> cellPositions, Color color)
    {
        foreach (var cellPosition in cellPositions)
        {
            overlaysTilemap.SetTile(cellPosition, blankTile);
            overlaysTilemap.SetTileFlags(cellPosition, TileFlags.None);
            overlaysTilemap.SetColor(cellPosition, color);
        }
    }

    public void ClearOverlay()
    {
        overlaysTilemap.ClearAllTiles();
    }

    private void HoverCell(Vector3Int cell)
    {
        if (gridObjects.ContainsKey(cell) && gridObjects[cell] != hoveredObject)
        {
            hoveredObject?.Unhover();
            hoveredObject = gridObjects[cell];
            hoveredObject.Hover();
        }
        else if (!gridObjects.ContainsKey(cell))
        {
            hoveredObject?.Unhover();
            hoveredObject = null;
        }
    }

    private void ClickCell(Vector3Int cell)
    {
        if (selectedObject == null)
        {
            SelectCell(cell);
        }
        else
        {
            var performedSomeAction = false;
            if (whoseTurn == Turn.Mine)
            {
                performedSomeAction = selectedObject.ClickCell(cell);
            }

            if (!performedSomeAction)
            {
                SelectCell(cell);
            }
        }
    }

    public void SelectCell(Vector3Int cell)
    {
        var obj = GetAtPosition(cell);
        if (obj == null || obj.owner != GridObject.Owner.Mine)
        {
            return;
        }

        // We don't care if the selected cell is already selected. It can just go through
        // its selection process again, which includes resetting the selection mode.
        overlaysTilemap.ClearAllTiles();

        selectedObject?.Deselect();
        selectedObject = obj;
        selectedObject?.Select();
        overlaysTilemap.SetTile(cell, selectTile);
    }

    public void DeselectCurrentItem()
    {
        selectedObject = null;
        overlaysTilemap.ClearAllTiles();
    }

    private void ProgressTimeOfDay(Turn whoseTurnIsStarting)
    {
        if (whoseTurnIsStarting == whoStarted)
        {
            roundsInCurrentTimeOfDay += 1;

            if (roundsInCurrentTimeOfDay >= 2)
            {
                roundsInCurrentTimeOfDay = 0;
                if (timeOfDay == TimeOfDay.Daytime)
                {
                    SoundManager.instance.TransitionMusic(1, 5);
                    SoundManager.instance.PlayClip(nightSound);
                    timeOfDay = TimeOfDay.Nighttime;
                }
                else
                {
                    SoundManager.instance.TransitionMusic(0, 5);
                    SoundManager.instance.PlayClip(daySound);
                    timeOfDay = TimeOfDay.Daytime;
                }
            }
        }

        Debug.Log($"Time of day is {timeOfDay}");
    }

    public void StartTurn()
    {
        Debug.Log("Starting turn");
        ProgressTimeOfDay(Turn.Mine);

        whoseTurn = Turn.Mine;
        foreach (var item in gridObjects.Values)
        {
            item.StartTurn(timeOfDay);
        }
    }

    public void EndTurn()
    {
        Debug.Log("Ending Turn");
        DeselectCurrentItem();

        whoseTurn = Turn.Theirs;
        foreach (var item in gridObjects.Values)
        {
            item.EndTurn();
        }

        ProgressTimeOfDay(Turn.Theirs);

        commands.EndTurn();
    }

    private void ReceiveTurn(TurnMessage message)
    {
        Debug.Log("Received Turn");

        foreach (CommandData item in message.commands)
        {
            item.ToCommand().Execute();
        }
        StartTurn();
    }

    void Update()
    {
        if (instance == null)
            return; // Hacky way to check that init happened

        var currentHoveredCell = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        currentHoveredCell = new Vector3Int(currentHoveredCell.x, currentHoveredCell.y, 0);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HoverCell(currentHoveredCell);

            if (Input.GetMouseButtonDown(0))
            {
                ClickCell(currentHoveredCell);
            }

            // TODO: UI feedback if we failed to change mode (because the action/move was already used)
            if (Input.GetKeyDown(KeyCode.M))
            {
                selectedObject?.EnterMoveMode();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedObject?.EnterActionMode(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectedObject?.EnterActionMode(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectedObject?.EnterActionMode(2);
            }

            if (whoseTurn == Turn.Mine)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    EndTurn();
                }
            }
        }
    }
}
