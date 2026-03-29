using UnityEngine;

[DisallowMultipleComponent]
public class HookHitAudio : MonoBehaviour
{
    public static HookHitAudio Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hookHitClip;
    [SerializeField, Min(0f)] private float cooldownSeconds = 0.05f;

    private float _lastPlayTime = float.NegativeInfinity;

    public static void RequestPlay()
    {
        if (Instance == null)
            return;

        Instance.PlayHookHit();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple HookHitAudio instances found. Using the first instance.", this);
            return;
        }

        Instance = this;

        if (audioSource == null && !TryGetComponent(out audioSource))
            audioSource = gameObject.AddComponent<AudioSource>();

        ConfigureAudioSource();
    }

    private void OnValidate()
    {
        if (audioSource == null)
            TryGetComponent(out audioSource);

        ConfigureAudioSource();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void PlayHookHit()
    {
        if (audioSource == null || hookHitClip == null)
            return;

        if (Time.unscaledTime < _lastPlayTime + cooldownSeconds)
            return;

        audioSource.PlayOneShot(hookHitClip);
        _lastPlayTime = Time.unscaledTime;
    }

    private void ConfigureAudioSource()
    {
        if (audioSource == null)
            return;

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }
}
