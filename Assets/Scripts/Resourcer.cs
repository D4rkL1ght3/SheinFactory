using UnityEngine;

public class Resourcer : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private MaterialType selectedMaterial = MaterialType.Stone;

    [SerializeField] private float spawnInterval = 1f;

    [Header("Material Prefab")]
    [SerializeField] private MaterialItem materialPrefab;

    [Header("UI")]
    [SerializeField] private ResourceSelectionPanel resourceSelectionPanel;

    private float spawnTimer;

    private void Awake()
    {
        if (resourceSelectionPanel == null)
        {
            resourceSelectionPanel = FindAnyObjectByType<ResourceSelectionPanel>();
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TryOutputMaterials();
        }
    }

    private void TryOutputMaterials()
    {
        TryOutput(Vector2Int.up);
        TryOutput(Vector2Int.right);
        TryOutput(Vector2Int.down);
        TryOutput(Vector2Int.left);
    }

    private void TryOutput(Vector2Int direction)
    {
        Vector2 worldPosition = (Vector2)transform.position + direction;

        Collider2D hit = Physics2D.OverlapPoint(worldPosition);

        if (hit == null)
            return;

        PlaceableTool tool = hit.GetComponent<PlaceableTool>();

        if (tool == null)
            return;

        MaterialItem newItem = Instantiate(
            materialPrefab,
            transform.position,
            Quaternion.identity
        );

        newItem.SetMaterialType(selectedMaterial);

        bool accepted = tool.ReceiveMaterial(newItem);

        if (!accepted)
        {
            Destroy(newItem.gameObject);
        }
    }

    public void SetMaterial(MaterialType material)
    {
        selectedMaterial = material;
    }

    public MaterialType GetMaterial()
    {
        return selectedMaterial;
    }

    private void OnMouseDown()
    {
        resourceSelectionPanel.Open(this);
    }
}