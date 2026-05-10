using UnityEngine;
using UnityEngine.SceneManagement;
// Class for game over functions (retry and exit)
public class GameOverManager : MonoBehaviour
{
    // exit button
    public void ExitToMainMenu()
    {
        if (SceneFade.Instance == null)
        {
            Debug.LogWarning("SceneFade missing — loading scene without fade.");
            SceneManager.LoadScene("MainMenu");
            return;
        }
        // loads main menu
        SceneFade.Instance.LoadScene("MainMenu");
        ResetRunState();
    }
    // retry button 
    public void Retry()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.ResetRun();
        else
            Debug.LogWarning("GameStateManager missing on Retry"); // logs if not working

        if (SceneFade.Instance != null)
            SceneFade.Instance.LoadScene("Game");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game"); // if instance not working load manually.
        ResetRunState();
    }
    // resets base stats
    void ResetRunState()
    {
        Player.Instance.ResetToBase();
        //logs
        Debug.Log("Run reset for retry.");
    }
}