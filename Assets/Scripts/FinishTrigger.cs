using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FinishTrigger : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI finalTimeText;

    public GameObject ResultPanel;

    [Header("Medal Objects")]
    public GameObject goldMedal;
    public GameObject silverMedal;
    public GameObject bronzeMedal;

    [Header("Medal Settings")]
    public float goldTime = 10f;
    public float silverTime = 20f;
    public float bronzeTime = 30f;

    [Header("Audio")]
    public AudioClip finishSound;
    public AudioClip medalSound;      // Один звук для всех медалей

    private AudioSource _audioSource;
    private bool _finished = false;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f;
        _audioSource.minDistance = 1f;
        _audioSource.maxDistance = 20f;
    }

    void OnTriggerEnter(Collider other)
    {
        ResultPanel.SetActive(true);

        if (_finished) return;
        if (other.GetComponent<CheckpointTrigger>() == null) return;

        _finished = true;

        GameTimer.Instance.StopTimer();
        float finalTime = GameTimer.Instance.GetTime();

        finalTimeText.text = "Final Time: " + FormatTime(finalTime);

        MedalType medal = GetMedal(finalTime);

        UpdateMedalSlots(medal);
        StartCoroutine(PlaySounds(medal));
    }

    IEnumerator PlaySounds(MedalType medal)
    {
        // Звук финиша сразу
        if (finishSound != null)
            _audioSource.PlayOneShot(finishSound);

        if (medalSound == null) yield break;

        switch (medal)
        {
            case MedalType.Bronze:
                // Бронза появляется сразу — звук без задержки
                _audioSource.PlayOneShot(medalSound);
                break;

            case MedalType.Silver:
                // Бронза сразу, серебро через 1с
                _audioSource.PlayOneShot(medalSound);
                yield return new WaitForSeconds(1.2f);
                _audioSource.PlayOneShot(medalSound);
                break;

            case MedalType.Gold:
                // Бронза сразу, серебро через 1с, золото через 2с
                _audioSource.PlayOneShot(medalSound);
                yield return new WaitForSeconds(1.2f);
                _audioSource.PlayOneShot(medalSound);
                yield return new WaitForSeconds(1.2f);
                _audioSource.PlayOneShot(medalSound);
                break;
        }
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