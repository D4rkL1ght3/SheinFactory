using UnityEngine;
using static ProcessorTool;

public class MixerTool : PlaceableTool
{
    [System.Serializable]
    public struct MixerRecipe
    {
        public MaterialType inputA;
        public MaterialType inputB;
        public MaterialType output;
    }

    [Header("Recipes")]
    [SerializeField] private MixerRecipe[] recipes;

    [Header("Processing")]
    [SerializeField] private float processingTime = 1f;

    [Header("References")]
    [SerializeField] private Transform itemAnchorA;
    [SerializeField] private Transform itemAnchorB;
    [SerializeField] private Transform outputPoint;

    private MaterialItem storedItemA;
    private MaterialItem storedItemB;

    private MixerRecipe currentRecipe;

    private float processingTimer;
    private bool isProcessing;
    private bool isProcessed;

    public override bool ReceiveMaterial(MaterialItem item)
    {
        if (isProcessing)
            return false;

        if (!IsMaterialUsed(item.MaterialType))
            return false;

        // Prevent duplicate materials
        if (storedItemA != null &&
            storedItemA.MaterialType == item.MaterialType)
            return false;

        if (storedItemB != null &&
            storedItemB.MaterialType == item.MaterialType)
            return false;

        if (storedItemA == null)
        {
            storedItemA = item;
            storedItemA.transform.position = itemAnchorA.position;
        }
        else if (storedItemB == null)
        {
            storedItemB = item;
            storedItemB.transform.position = itemAnchorB.position;
        }
        else
        {
            return false;
        }

        TryStartProcessing();

        return true;
    }

    private bool TryGetRecipe(
    MaterialType first,
    MaterialType second,
    out MixerRecipe recipe)
    {
        foreach (MixerRecipe r in recipes)
        {
            bool normal =
                r.inputA == first &&
                r.inputB == second;

            bool reversed =
                r.inputA == second &&
                r.inputB == first;

            if (normal || reversed)
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
        if (storedItemA == null)
            return;

        if (isProcessing)
        {
            ProcessItems();
        }
        else if (isProcessed)
        {
            MoveOutputItem();
        }
    }

    private void TryStartProcessing()
    {
        if (storedItemA == null || storedItemB == null)
            return;

        if (TryGetRecipe(
            storedItemA.MaterialType,
            storedItemB.MaterialType,
            out currentRecipe))
        {
            processingTimer = 0f;
            isProcessing = true;
        }
    }

    private void ProcessItems()
    {
        processingTimer += Time.deltaTime;

        if (processingTimer < processingTime)
            return;

        Destroy(storedItemB.gameObject);

        storedItemA.SetMaterialType(currentRecipe.output);

        isProcessing = false;
        isProcessed = true;
    }

    private void MoveOutputItem()
    {
        storedItemA.transform.position = Vector3.MoveTowards(
            storedItemA.transform.position,
            outputPoint.position,
            Time.deltaTime * 2f
        );

        if (Vector3.Distance(storedItemA.transform.position, outputPoint.position) <= 0.01f)
        {
            TryTransferItem();
        }
    }

    private bool IsMaterialUsed(MaterialType material)
    {
        foreach (MixerRecipe recipe in recipes)
        {
            if (recipe.inputA == material ||
                recipe.inputB == material)
                return true;
        }

        return false;
    }

    private void TryTransferItem()
    {
        PlaceableTool nextTool =
            GridManager.Instance.GetToolAtGridPosition(GetOutputGridPosition());

        if (nextTool == null)
            return;

        if (nextTool.ReceiveMaterial(storedItemA))
        {
            storedItemA = null;
            storedItemB = null;

            processingTimer = 0f;
            isProcessing = false;
            isProcessed = false;
        }
    }
}