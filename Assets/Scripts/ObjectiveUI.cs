using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform objectiveParent;

    [SerializeField] private GameObject objectivePrefab;

    private readonly List<GameObject> spawnedObjectives = new();

    public void Refresh(List<ObjectiveData> objectives)
    {
        Clear();

        foreach (ObjectiveData objective in objectives)
        {
            GameObject row = Instantiate(
                objectivePrefab,
                objectiveParent);

            ObjectiveUIRow ui =
                row.GetComponent<ObjectiveUIRow>();

            ui.Setup(objective);

            spawnedObjectives.Add(row);
        }
    }

    private void Clear()
    {
        foreach (GameObject row in spawnedObjectives)
        {
            Destroy(row);
        }

        spawnedObjectives.Clear();
    }
}