using UnityEngine;

public class Extractor : PlaceableTool
{
    [SerializeField] private ObjectiveManager objectiveManager;

    private void Awake()
    {
        if (objectiveManager == null)
            objectiveManager = FindAnyObjectByType<ObjectiveManager>();
    }

    public override bool ReceiveMaterial(MaterialItem item)
    {
        if (objectiveManager == null)
            return false;

        bool accepted = objectiveManager.DeliverMaterial(item.MaterialType);

        if (accepted)
        {
            Destroy(item.gameObject);
        }

        return accepted;
    }
}