using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Checkpoint : MonoBehaviour
{
    [Header("Settings")]
    public Transform spawnPoint;

    [Header("Animation")]
    public Animator animator;

    [Header("Audio")]
    public AudioClip activationSound;
    private AudioSource _audioSource;

    private bool _isActivated = false;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1f;   // 3D-звук для VR
        _audioSource.minDistance = 1f;
        _audioSource.maxDistance = 15f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isActivated) return;

        Debug.Log("Кто-то вошёл в триггер: " + other.name);

        if (other.GetComponent<CheckpointTrigger>() == null) return;

        Debug.Log("Чекпоинт активирован!");

        _isActivated = true;

        if (animator != null)
            animator.SetTrigger("Activate");

        // Воспроизвести звук
        if (activationSound != null)
            _audioSource.PlayOneShot(activationSound);

        CheckpointManager.Instance.SetCheckpoint(spawnPoint.position);
    }
}