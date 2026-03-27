using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    private float _time;
    private bool _isRunning = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (!_isRunning) return;

        _time += Time.deltaTime;
        UpdateUI();
    }

    public void StartTimer()
    {
        _time = 0f;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public float GetTime()
    {
        return _time;
    }

    void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time % 60);
        int milliseconds = Mathf.FloorToInt((_time * 100) % 100);

        timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}
