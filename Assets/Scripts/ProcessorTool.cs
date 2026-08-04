using UnityEngine;

public class ProcessorTool : PlaceableTool
{
    [System.Serializable]
    public struct MaterialRecipe
    {
        public MaterialType inputMaterial;
        public MaterialType outputMaterial;
    }

    [Header("Processing Settings")]
    [SerializeField] private MaterialRecipe[] recipes;
    [SerializeField] private float processingTime = 1f;

    [Header("References")]
    [SerializeField] private Transform itemAnchor;
    [SerializeField] private Transform outputPoint;

    private MaterialItem currentItem;
    private MaterialRecipe currentRecipe;

    private float processingTimer;
    private bool isProcessed;

    public bool IsOccupied => currentItem != null;

    public override bool ReceiveMaterial(MaterialItem item)
    {
        if (currentItem != null)
            return false;

        if (!TryGetRecipe(item.MaterialType, out currentRecipe))
            return false;

        currentItem = item;

        currentItem.transform.position = itemAnchor.position;

        processingTimer = 0f;
        isProcessed = false;

        return true;
    }

    private bool TryGetRecipe(MaterialType inputMaterial, out MaterialRecipe recipe)
    {
        foreach (MaterialRecipe r in recipes)
        {
            if (r.inputMaterial == inputMaterial)
            {
                recipe = r;
                return true;
            }
        }

        recipe = default;
        return false;
    }

    private void Update()
    {
        if (currentItem == null)
            return;

        if (!isProcessed)
        {
            ProcessItem();
        }
        else
        {
            MoveToOutput();
        }
    }

    private void ProcessItem()
    {
        processingTimer += Time.deltaTime;

        if (processingTimer >= processingTime)
        {
            currentItem.SetMaterialType(currentRecipe.outputMaterial);
            isProcessed = true;
        }
    }

    private void MoveToOutput()
    {
        currentItem.transform.position = Vector3.MoveTowards(
            currentItem.transform.position,
            outputPoint.position,
            Time.deltaTime * 2f
        );

        if (Vector3.Distance(currentItem.transform.position, outputPoint.position) <= 0.01f)
        {
            TryTransferItem();
        }
    }

    private void TryTransferItem()
    {
        PlaceableTool nextTool =
            GridManager.Instance.GetToolAtGridPosition(GetOutputGridPosition());

        if (nextTool == null)
            return;

        if (nextTool.ReceiveMaterial(currentItem))
        {
            currentItem = null;
            processingTimer = 0f;
            isProcessed = false;
        }
    }
}