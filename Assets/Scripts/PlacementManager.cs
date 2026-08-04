using UnityEngine.EventSystems;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    private enum EditMode
    {
        None,
        Placement,
        Remove
    }

    private EditMode currentMode = EditMode.None;

    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform placedToolParent;

    [Header("Current Tool")]
    [SerializeField] private GameObject currentToolPrefab;

    [Header("Placement Preview")]
    private GameObject previewObject;

    private PlaceableTool highlightedTool;
    private SpriteRenderer highlightedRenderer;

    private Vector2Int lastPlacedTile = new Vector2Int(int.MinValue, int.MinValue);

    private int rotationIndex = 0;

    private void Update()
    {
        HandlePreview();
        HandleRotation();
        HandlePlacement();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMode == EditMode.Placement)
                ExitPlacementMode();

            else if (currentMode == EditMode.Remove)
                ExitRemoveMode();
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastPlacedTile = new Vector2Int(int.MinValue, int.MinValue);
        }

        if (currentMode == EditMode.Remove)
        {
            UpdateRemoveMode();
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rotationIndex++;

            if (rotationIndex > 3)
                rotationIndex = 0;
        }
    }

    private void HandlePlacement()
    {
        if (currentMode != EditMode.Placement)
            return;

        if (IsPointerOverUI())
            return;

        if (currentToolPrefab == null)
            return;

        if (!Input.GetMouseButton(0))
            return;

        Vector3 snappedPos = gridManager.GetSnappedMousePosition();
        Vector2Int gridPos = gridManager.WorldToGrid(snappedPos);

        if (gridPos == lastPlacedTile)
            return;

        lastPlacedTile = gridPos;

        if (IsTileOccupied(gridPos))
            return;

        Quaternion rotation = Quaternion.Euler(0f, 0f, rotationIndex * 90f);

        GameObject obj = Instantiate(
            currentToolPrefab,
            snappedPos,
            rotation,
            placedToolParent);

        PlaceableTool tool = obj.GetComponent<PlaceableTool>();

        if (tool != null)
        {
            tool.Initialize(gridPos, rotationIndex);

            gridManager.RegisterTool(gridPos, tool);
        }
    }

    private bool IsTileOccupied(Vector2Int gridPos)
    {
        return gridManager.IsOccupied(gridPos);
    }

    public void SetCurrentTool(GameObject prefab)
    {
        Debug.Log("Tool selected: " + prefab.name);

        currentToolPrefab = prefab;

        EnterPlacementMode();
    }

    private void EnterPlacementMode()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = Instantiate(currentToolPrefab);

        PreparePreview(previewObject);

        currentMode = EditMode.Placement;
    }

    private void ExitPlacementMode()
    {
        Debug.Log("Exited placement mode.");

        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = null;
        currentToolPrefab = null;

        currentMode = EditMode.None;
    }

    public void EnterRemoveMode()
    {
        ExitPlacementMode();

        currentMode = EditMode.Remove;
    }

    private void ExitRemoveMode()
    {
        ClearHighlightedTool();

        currentMode = EditMode.None;
    }

    private void UpdateRemoveMode()
    {
        if (IsPointerOverUI())
        {
            ClearHighlightedTool();
            return;
        }

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int gridPos = gridManager.WorldToGrid(mouseWorld);

        PlaceableTool tool = gridManager.GetToolAtGridPosition(gridPos);

        if (tool != highlightedTool)
        {
            HighlightTool(tool);
        }

        if (tool != null && Input.GetMouseButton(0))
        {
            DeleteTool(tool);
        }
    }

    private void HighlightTool(PlaceableTool tool)
    {
        ClearHighlightedTool();

        highlightedTool = tool;

        if (highlightedTool == null)
            return;

        highlightedRenderer = highlightedTool.GetComponent<SpriteRenderer>();

        if (highlightedRenderer != null)
            highlightedRenderer.color = Color.red;
    }

    private void ClearHighlightedTool()
    {
        if (highlightedRenderer != null)
            highlightedRenderer.color = Color.white;

        highlightedRenderer = null;
        highlightedTool = null;
    }

    private void DeleteTool(PlaceableTool tool)
    {
        gridManager.UnregisterTool(tool.GridPosition);

        Destroy(tool.gameObject);

        ClearHighlightedTool();
    }

    private void HandlePreview()
    {
        if (currentMode != EditMode.Placement)
            return;

        if (IsPointerOverUI())
        {
            previewObject.SetActive(false);
            return;
        }
        else
        {
            previewObject.SetActive(true);
        }

        Vector3 snappedPos = gridManager.GetSnappedMousePosition();
        Vector2Int gridPos = gridManager.WorldToGrid(snappedPos);

        previewObject.transform.position = snappedPos;
        previewObject.transform.rotation = Quaternion.Euler(0f, 0f, rotationIndex * 90f);

        SpriteRenderer sprite = previewObject.GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
        {
            sprite.color = IsTileOccupied(gridPos)
                ? new Color(1f, 0.4f, 0.4f, 0.5f)
                : new Color(1f, 1f, 1f, 0.5f);
        }
    }

    private void PreparePreview(GameObject preview)
    {
        foreach (Collider2D col in preview.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
    }
}