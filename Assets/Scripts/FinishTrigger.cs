using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FinishTrigger : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI finalTimeText;

    [Header("Medal Objects")]
    public GameObject goldMedal;
    public GameObject silverMedal;
    public GameObject bronzeMedal;

    [Header("Medal Settings")]
    public float goldTime = 60f;
    public float silverTime = 90f;
    public float bronzeTime = 120f;

    private bool _finished = false;

    void OnTriggerEnter(Collider other)
    {
        if (_finished) return;
        if (other.GetComponent<CheckpointTrigger>() == null) return;

        _finished = true;

        GameTimer.Instance.StopTimer();
        float finalTime = GameTimer.Instance.GetTime();

        finalTimeText.text = "Final Time: " + FormatTime(finalTime);

        MedalType medal = GetMedal(finalTime);

        UpdateMedalSlots(medal);
    }

    MedalType GetMedal(float time)
    {
        if (time <= goldTime) return MedalType.Gold;
        if (time <= silverTime) return MedalType.Silver;
        if (time <= bronzeTime) return MedalType.Bronze;

        return MedalType.None;
    }

    void UpdateMedalSlots(MedalType medal)
    {
        // Включаем по результату
        if (medal == MedalType.Gold)
        {
            goldMedal.SetActive(true);
            silverMedal.SetActive(true);
            bronzeMedal.SetActive(true);
        }
        else if (medal == MedalType.Silver)
        {
            silverMedal.SetActive(true);
            bronzeMedal.SetActive(true);
        }
        else if (medal == MedalType.Bronze)
        {
            bronzeMedal.SetActive(true);
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);

        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}