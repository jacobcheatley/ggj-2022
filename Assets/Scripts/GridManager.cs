using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

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
    private Vector3Int selected;
    private Vector3Int hovered;

    private Grid grid;
    [Header("Tiles")]
    //[SerializeField]
    //private Tilemap terrain;
    [SerializeField]
    private Tilemap overlaysTilemap;
    [SerializeField]
    private Tile selectTile;

    private bool hovering;

    private void Start()
    {
        grid = GetComponent<Grid>();

        foreach (var spawnPosition in team1SpawnPositions)
        {
            var gridObject = SpawnGridObject(testStartObject, spawnPosition);
            gridObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0);
        }

        foreach (var spawnPosition in team2SpawnPositions)
        {
            var gridObject = SpawnGridObject(testStartObject, spawnPosition);
            gridObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 1);
        }
    }

    public GridObject SpawnGridObject(GameObject otherPrefab, Vector2Int cellPosition)
    {
        return SpawnGridObject(otherPrefab, new Vector3Int(cellPosition.x, cellPosition.y, 0));
    }

    public GridObject SpawnGridObject(GameObject otherPrefab, Vector3Int cellPosition)
    {
        //var cellPosition = new Vector3Int(x, y, 0);
        gridObjects[cellPosition] = GameObject.Instantiate(otherPrefab, grid.CellToWorld(cellPosition), Quaternion.identity).GetComponent<GridObject>();
        return gridObjects[cellPosition];
    }

    private void HoverCell(Vector3Int cell)
    {
        if (cell != hovered)
        {
            if (gridObjects.ContainsKey(hovered))
                gridObjects[hovered].Unhover();

            if (gridObjects.ContainsKey(cell))
                gridObjects[cell].Hover();
        }

        hovered = cell;
    }

    private void SelectCell(Vector3Int cell)
    {
        overlaysTilemap.ClearAllTiles();
        overlaysTilemap.SetTile(cell, selectTile);

        if (cell != selected)
        {
            if (gridObjects.ContainsKey(selected))
                gridObjects[selected].Deselect();

            if (gridObjects.ContainsKey(cell))
                gridObjects[cell].Select();
        }

        selected = cell;
    }

    // Update is called once per frame
    void Update()
    {
        hovering = !EventSystem.current.IsPointerOverGameObject();

        var currentHoveredCell = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        currentHoveredCell = new Vector3Int(currentHoveredCell.x, currentHoveredCell.y, 0);

        if (hovering)
        {
            HoverCell(currentHoveredCell);

            if (Input.GetMouseButtonDown(0))
            {
                SelectCell(currentHoveredCell);
            }
        }
    }
}
