using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // loads next scene function
    public void GoToScene(string sceneName) {
        SceneFade.Instance.LoadScene(sceneName);
    }

    // quit the game
    public void QuitApp() {
        Application.Quit();
        Debug.Log("Application has quit.");
    }
    
    // set fullscreen or windowed
    public void SetFullscreen()
    {
        Screen.fullScreen = true;
    }

    public void SetWindowed()
    {
        Screen.fullScreen = false;
    }
}