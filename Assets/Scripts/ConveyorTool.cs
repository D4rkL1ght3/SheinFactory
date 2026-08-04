using UnityEngine;

public class ConveyorTool : PlaceableTool
{
    [Header("Conveyor Settings")]
    [SerializeField] private Transform itemAnchor;
    [SerializeField] private Transform outputPoint;
    [SerializeField] private float moveSpeed = 2f;

    private MaterialItem currentItem;

    public bool IsOccupied => currentItem != null;

    public MaterialItem GetCurrentItem()
    {
        return currentItem;
    }

    public override bool ReceiveMaterial(MaterialItem item)
    {
        if (currentItem != null)
            return false;

        currentItem = item;

        currentItem.transform.position = itemAnchor.position;

        return true;
    }

    private void Update()
    {
        if (currentItem == null)
            return;

        MoveCurrentItem();

        if (Vector3.Distance(currentItem.transform.position, outputPoint.position) <= 0.01f)
        {
            TryTransferItem();
        }
    }

    private void MoveCurrentItem()
    {
        currentItem.transform.position = Vector3.MoveTowards(
            currentItem.transform.position,
            outputPoint.position,
            moveSpeed * Time.deltaTime
        );
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
        }
    }
}