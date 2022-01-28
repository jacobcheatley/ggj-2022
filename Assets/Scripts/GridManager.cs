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

    enum Turn
    {
        Mine,
        Theirs
    }

    private Turn whoseTurn;

    public static GridManager instance;

    private void Start()
    {
        instance = this;
        grid = GetComponent<Grid>();

        // TODO: Deterministic logic here.
        whoseTurn = Turn.Mine;

        foreach (var spawnPosition in team1SpawnPositions)
        {
            var gridObject = SpawnGridObject(testStartObject, spawnPosition);
            gridObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0);
            // TODO: Make sure we're only starting turn if it's our turn later on
            gridObject.StartTurn();
        }

        foreach (var spawnPosition in team2SpawnPositions)
        {
            var gridObject = SpawnGridObject(testStartObject, spawnPosition);
            gridObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 1);
            gridObject.StartTurn();
        }
    }

    public GridObject SpawnGridObject(GameObject gridObjectPrefab, Vector2Int cellPosition)
    {
        return SpawnGridObject(gridObjectPrefab, new Vector3Int(cellPosition.x, cellPosition.y, 0));
    }

    public GridObject SpawnGridObject(GameObject gridObjectPrefab, Vector3Int cellPosition)
    {
        gridObjects[cellPosition] = Instantiate(gridObjectPrefab, grid.CellToWorld(cellPosition), Quaternion.identity).GetComponent<GridObject>().Init(this, cellPosition);
        return gridObjects[cellPosition];
    }

    public void Move(Vector3Int fromCell, Vector3Int toCell)
    {
        if (PositionIsEmpty(toCell))
        {
            gridObjects[fromCell].Move(toCell);
        }
        else
        {
            Debug.Log("Can't move there.");
        }
    }

    public bool PositionIsEmpty(Vector3Int pos)
    {
        return !gridObjects.ContainsKey(pos);
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

    public List<Vector3Int> WithinMoveableCells(Vector3Int fromCell, float distance)
    {
        return WithinCells(fromCell, distance, excludeSelf: true, excludeOthers: true);
    }

    public List<Vector3Int> WithinActionableCells(Vector3Int fromCell, float distance)
    {
        return WithinCells(fromCell, distance, excludeSelf: true, excludeOthers: false);
    }

    public List<Vector3Int> WithinCells(
        Vector3Int fromCell,
        float distance,
        bool excludeSelf = false,
        bool excludeOthers = false
        )
    {
        // TODO: Probably need to Dijkstra's and figure out paths at some point - but not yet!
        var result = new List<Vector3Int>();
        float squaredDistance = distance * distance + 1f; // Extra 1 allows a cheeky little extra diagonal for free

        for (int x = (int)-distance; x <= (int)distance; x++)
        {
            for (int y = (int)-distance; y <= (int)distance; y++)
            {
                if (excludeSelf && x == 0 && y == 0)
                {
                    continue;
                }

                var targetCell = fromCell + Vector3Int.right * x + Vector3Int.up * y;
                if (excludeOthers && !PositionIsEmpty(targetCell))
                {
                    continue;
                }

                if (x * x + y * y <= squaredDistance)
                {
                    result.Add(targetCell);
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
            var performedSomeAction = selectedObject.ClickCell(cell);
            if (!performedSomeAction)
            {
                SelectCell(cell);
            }
        }
    }

    public void SelectCell(Vector3Int cell)
    {
        overlaysTilemap.ClearAllTiles();
        // We don't care if the selected cell is already selected. It can just go through
        // its selection process again, which includes resetting the selection mode.
        if (gridObjects.ContainsKey(cell))
        {
            selectedObject?.Deselect();
            selectedObject = gridObjects[cell];
            selectedObject.Select();
        }
        else if (!gridObjects.ContainsKey(cell))
        {
            selectedObject?.Deselect();
            selectedObject = null;
        }

        overlaysTilemap.SetTile(cell, selectTile);
    }

    public void DeselectCurrentItem()
    {
        selectedObject = null;
    }

    public void StartTurn()
    {
        Debug.Log("Starting turn");

        whoseTurn = Turn.Mine;
        foreach (var item in gridObjects.Values)
        {
            item.StartTurn();
        }
    }

    public void EndTurn()
    {
        Debug.Log("Ending Turn");

        whoseTurn = Turn.Theirs;
        foreach (var item in gridObjects.Values)
        {
            item.EndTurn();
        }

        // Until we have networking, their turn will immediately end.
        StartTurn();
    }

    void Update()
    {
        var currentHoveredCell = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        currentHoveredCell = new Vector3Int(currentHoveredCell.x, currentHoveredCell.y, 0);

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            HoverCell(currentHoveredCell);

            if (Input.GetMouseButtonDown(0))
            {
                ClickCell(currentHoveredCell);
            }

            if (whoseTurn == Turn.Mine && Input.GetKeyDown(KeyCode.Space))
            {
                EndTurn();
            }

            // TODO: UI feedback if we failed to change mode (because the action/move was already used)
            if (whoseTurn == Turn.Mine && Input.GetKeyDown(KeyCode.M))
            {
                selectedObject?.EnterMoveMode();
            }

            if (whoseTurn == Turn.Mine && Input.GetKeyDown(KeyCode.A))
            {
                selectedObject?.EnterActionMode();
            }
        }
    }
}
