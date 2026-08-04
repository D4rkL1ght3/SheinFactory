using UnityEngine;

[System.Serializable]
public class ObjectiveData
{
    [Header("Objective")]
    public MaterialType targetMaterial;

    public int requiredAmount;

    [HideInInspector]
    public int deliveredAmount = 0;

    [HideInInspector]
    public bool objectiveCompleted = false;

    public float GetProgress()
    {
        if (requiredAmount <= 0)
            return 0f;

        return (float)deliveredAmount / requiredAmount;
    }

    public void AddProgress(int amount)
    {
        deliveredAmount += amount;

        if (deliveredAmount >= requiredAmount)
        {
            deliveredAmount = requiredAmount;
            objectiveCompleted = true;
        }
    }

    public bool IsComplete()
    {
        return objectiveCompleted;
    }

    public void Reset()
    {
        deliveredAmount = 0;
        objectiveCompleted = false;
    }
}