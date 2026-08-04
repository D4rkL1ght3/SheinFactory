using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        Debug.Log("Starting Game...");
    }
    
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
