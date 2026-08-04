using Unity.VisualScripting;
using UnityEngine;

public class Extractor : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;

    private void Awake()
    {
        if (objectiveManager == null)
            objectiveManager = FindAnyObjectByType<ObjectiveManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MaterialItem material =
            other.GetComponent<MaterialItem>();

        if (material == null)
            return;

        bool accepted =
            objectiveManager.DeliverMaterial(material.MaterialType);

        if (accepted)
        {
            Destroy(material.gameObject);
        }
    }
}