using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject extractorPrefab;
    [SerializeField] private GameObject resourcerPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform levelParent;
    [SerializeField] private Vector2Int mapSize = new Vector2Int(20, 20);

    private readonly List<GameObject> spawnedObjects = new();
    private readonly HashSet<Vector2Int> occupiedPositions = new();

    public void GenerateLevel(LevelData levelData)
    {
        ClearLevel();

        SpawnExtractor();

        for (int i = 0; i < levelData.resourcerCount; i++)
        {
            SpawnResourcer();
        }
    }

    private void ClearLevel()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);

            PlaceableTool tool = obj.GetComponent<PlaceableTool>();

            if (tool != null)
                GridManager.Instance.UnregisterTool(tool.GridPosition);
        }

        spawnedObjects.Clear();
        occupiedPositions.Clear();

        // TODO:
        // PlacementManager.Instance.ClearPlacedTools();
        // GridManager.Instance.ClearOccupiedTiles();
    }

    private void SpawnExtractor()
    {
        Vector2Int gridPos = GetRandomGridPosition();

        Vector3 worldPos = GridManager.Instance.GridToWorld(gridPos);

        GameObject extractorObj = Instantiate(
            extractorPrefab,
            worldPos,
            Quaternion.identity,
            levelParent);

        // Register the extractor as a PlaceableTool
        Extractor extractor = extractorObj.GetComponent<Extractor>();

        if (extractor != null)
        {
            extractor.Initialize(gridPos, 0);

            GridManager.Instance.RegisterTool(gridPos, extractor);
        }

        spawnedObjects.Add(extractorObj);

        occupiedPositions.Add(gridPos);
    }

    private void SpawnResourcer()
    {
        Vector2Int gridPos = GetRandomGridPosition();

        Vector3 worldPos = GridManager.Instance.GridToWorld(gridPos);

        GameObject resourcer = Instantiate(
            resourcerPrefab,
            worldPos,
            Quaternion.identity,
            levelParent);

        spawnedObjects.Add(resourcer);

        occupiedPositions.Add(gridPos);

        // Optional
        // GridManager.Instance.SetOccupied(gridPos, true);
    }

    private Vector2Int GetRandomGridPosition()
    {
        Vector2Int position;

        do
        {
            position = new Vector2Int(
                Random.Range(0, mapSize.x),
                Random.Range(0, mapSize.y));
        }
        while (occupiedPositions.Contains(position));

        return position;
    }
}