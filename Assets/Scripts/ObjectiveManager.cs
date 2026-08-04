using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [Header("Current Objectives")]
    [SerializeField] private List<ObjectiveData> currentObjectives = new();

    [Header("Reference")]
    [SerializeField] private ObjectiveUI objectiveUI;

    public void GenerateObjectives(LevelData levelData)
    {
        currentObjectives.Clear();

        List<MaterialType> availableMaterials =
            new(levelData.availableObjectiveMaterials);

        Shuffle(availableMaterials);

        int objectiveCount = Mathf.Min(
            levelData.numberOfObjectives,
            availableMaterials.Count);

        for (int i = 0; i < objectiveCount; i++)
        {
            ObjectiveData objective = new ObjectiveData();

            objective.targetMaterial = availableMaterials[i];

            objective.requiredAmount = Random.Range(
                levelData.minimumObjectiveAmount,
                levelData.maximumObjectiveAmount + 1);

            objective.Reset();

            currentObjectives.Add(objective);
        }

        objectiveUI.Refresh(currentObjectives);
    }

    public bool DeliverMaterial(MaterialType material)
    {
        foreach (ObjectiveData objective in currentObjectives)
        {
            if (objective.targetMaterial != material)
                continue;

            if (objective.objectiveCompleted)
                return false;

            objective.AddProgress(1);

            objectiveUI.Refresh(currentObjectives);

            if (AreAllObjectivesComplete())
            {
                LevelManager.Instance.CompleteLevel();
            }

            return true;
        }

        return false;
    }

    public List<ObjectiveData> GetObjectives()
    {
        return currentObjectives;
    }

    private bool AreAllObjectivesComplete()
    {
        foreach (ObjectiveData objective in currentObjectives)
        {
            if (!objective.objectiveCompleted)
                return false;
        }

        return true;
    }

    private void Shuffle(List<MaterialType> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int random = Random.Range(0, i + 1);

            (list[i], list[random]) = (list[random], list[i]);
        }
    }
}