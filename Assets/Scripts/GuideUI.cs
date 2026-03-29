using UnityEngine;

public class GuideUI : MonoBehaviour
{
    public GameObject guidePanel;  

    public void ShowGuide()
    {
        guidePanel.SetActive(true);
    }

    public void HideGuide()
    {
        guidePanel.SetActive(false);
    }
}