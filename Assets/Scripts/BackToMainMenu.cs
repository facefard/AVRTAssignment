using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}