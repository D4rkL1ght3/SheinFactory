using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private LevelData[] levels;

    [SerializeField]
    private int currentLevelIndex = 0;

    [Header("References")]
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private LevelGenerator levelGenerator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartCurrentLevel();
    }

    public void StartCurrentLevel()
    {
        UnlockCurrentLevelTools();

        levelGenerator.GenerateLevel(GetCurrentLevelData());

        objectiveManager.GenerateObjectives(GetCurrentLevelData());
    }

    public void CompleteLevel()
    {
        if (HasNextLevel())
        {
            currentLevelIndex++;
            StartCurrentLevel();
        }
        else
        {
            GameCompleted();
        }
    }

    private void UnlockCurrentLevelTools()
    {
        foreach (GameObject button in GetCurrentLevelData().unlockedToolButtons)
        {
            if (button != null)
            {
                button.SetActive(true);
            }
        }
    }

    private void GameCompleted()
    {
        Debug.Log("Congratulations! All levels completed!");
        SceneManager.LoadScene("MainMenu");
    }

    public bool HasNextLevel()
    {
        return currentLevelIndex < levels.Length - 1;
    }

    public bool IsFinalLevel()
    {
        return currentLevelIndex >= levels.Length - 1;
    }

    public int GetCurrentLevel()
    {
        return currentLevelIndex + 1;
    }

    public LevelData GetCurrentLevelData()
    {
        return levels[currentLevelIndex];
    }
}