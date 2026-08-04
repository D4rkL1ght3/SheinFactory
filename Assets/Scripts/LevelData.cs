using UnityEngine;

[System.Serializable]
public class LevelData
{
    [Header("Objectives")]
    public int numberOfObjectives = 3;

    public int minimumObjectiveAmount = 10;

    public int maximumObjectiveAmount = 20;

    public MaterialType[] availableObjectiveMaterials;

    [Header("Level Generation")]
    public int resourcerCount = 3;

    [Header("Unlocked Tool Buttons")]
    public GameObject[] unlockedToolButtons;
}