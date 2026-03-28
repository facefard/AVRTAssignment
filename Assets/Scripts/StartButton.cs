using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [Tooltip("遷移先シーンのBuild Index")]
    public int targetSceneIndex = 1;
    public Button button;

    private void Start()
    {
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        SceneTransitionManager.singleton.GoToSceneAsync(targetSceneIndex);
    }
}
