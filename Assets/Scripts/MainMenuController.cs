using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public string mainSceneName = "Daniil 1";

    // 开始游戏
    public void StartGame()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    // 退出游戏
    public void QuitGame()
    {
        Debug.Log("Quit Game"); 

        Application.Quit(); 
    }
}