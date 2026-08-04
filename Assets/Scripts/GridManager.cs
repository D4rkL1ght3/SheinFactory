using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 1f;

    private Dictionary<Vector2Int, PlaceableTool> occupiedTiles =
    new Dictionary<Vector2Int, PlaceableTool>();

    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Returns the current mouse position in world space.
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        return mousePos;
    }

    // Converts a world position into grid coordinates.
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.y / cellSize);

        return new Vector2Int(x, y);
    }

    // Converts grid coordinates back into world space.
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * cellSize,
            gridPosition.y * cellSize,
            0f
        );
    }

    // Snaps a world position to the nearest grid cell.
    public Vector3 GetSnappedPosition(Vector3 worldPosition)
    {
        Vector2Int gridPos = WorldToGrid(worldPosition);
        return GridToWorld(gridPos);
    }

    // Returns the mouse position already snapped to the grid.
    public Vector3 GetSnappedMousePosition()
    {
        return GetSnappedPosition(GetMouseWorldPosition());
    }

    public void RegisterTool(Vector2Int gridPosition, PlaceableTool tool)
    {
        occupiedTiles[gridPosition] = tool;
    }

    public void UnregisterTool(Vector2Int gridPosition)
    {
        occupiedTiles.Remove(gridPosition);
    }

    public bool IsOccupied(Vector2Int gridPosition)
    {
        return occupiedTiles.ContainsKey(gridPosition);
    }

    public PlaceableTool GetToolAtGridPosition(Vector2Int gridPosition)
    {
        occupiedTiles.TryGetValue(gridPosition, out PlaceableTool tool);
        return tool;
    }
}