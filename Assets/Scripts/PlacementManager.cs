using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform placedToolParent;

    [Header("Current Tool")]
    [SerializeField] private GameObject currentToolPrefab;

    [Header("Placement Preview")]
    private GameObject previewObject;
    private bool isPlacementMode = false;

    private Vector2Int lastPlacedTile = new Vector2Int(int.MinValue, int.MinValue);

    private int rotationIndex = 0;

    private void Update()
    {
        HandlePreview();
        HandleRotation();
        HandlePlacement();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPlacementMode();
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastPlacedTile = new Vector2Int(int.MinValue, int.MinValue);
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
        if (!isPlacementMode)
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

        isPlacementMode = true;
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

        isPlacementMode = false;
    }

    private void HandlePreview()
    {
        if (!isPlacementMode)
            return;

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
}