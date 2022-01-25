using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    [Header("Map Generation")]
    // TODO spawn different things, generate a more interesting map, probably make this a scriptable object
    [SerializeField]
    private GameObject team1StartObject;
    [SerializeField]
    private GameObject team2StartObject;
    [SerializeField]
    private Vector2Int[] team1SpawnPositions;
    [SerializeField]
    private Vector2Int[] team2SpawnPositions;

    // TODO - cleverer area size, possibly
    private const int X = 8;
    private const int Y = 8;

    // TODO: Abstracted square info which has a building layer and creature layer, rather than just game objects
    private GameObject[,] gridItems = new GameObject[X, Y];

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
            SpawnGridItem(team1StartObject, spawnPosition);

        foreach (var spawnPosition in team2SpawnPositions)
            SpawnGridItem(team2StartObject, spawnPosition);
    }

    public void SpawnGridItem(GameObject otherPrefab, Vector2Int position)
    {
        SpawnGridItem(otherPrefab, position.x, position.y);
    }

    public void SpawnGridItem(GameObject otherPrefab, int x, int y)
    {
        gridItems[x, y] = GameObject.Instantiate(otherPrefab, grid.CellToWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
    }

    private void HoverCell(Vector3Int cell)
    {

    }

    private void SelectCell(Vector3Int cell)
    {
        overlaysTilemap.ClearAllTiles();
        overlaysTilemap.SetTile(cell, selectTile);
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
