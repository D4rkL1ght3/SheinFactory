using UnityEngine;

public class RecyclingBinTool : PlaceableTool
{
    public override bool ReceiveMaterial(MaterialItem item)
    {
        Destroy(item.gameObject);
        return true;
    }
}