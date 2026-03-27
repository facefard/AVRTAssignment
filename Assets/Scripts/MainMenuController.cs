using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string startSceneName = "SampleScene";
    public string checkpointSceneName = "FullMap"; 

    public void StartGame()
    {
        SceneManager.LoadScene(startSceneName);
    }

    public void GoToCheckpoint()
    {
        SceneManager.LoadScene(checkpointSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}