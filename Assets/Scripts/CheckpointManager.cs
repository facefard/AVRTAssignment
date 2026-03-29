using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [Header("Settings")]
    public float fallDeathHeight = -30f;
    public Transform vrPlayer;

    private Vector3 _lastCheckpoint;
    private bool _hasCheckpoint = false;

    void Awake()
    {
        Instance = this;
        _lastCheckpoint = vrPlayer.position;
    }

    void Update()
    {
        if (vrPlayer.position.y < fallDeathHeight)
        {
            Respawn();
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        _lastCheckpoint = position;
        _hasCheckpoint = true;
        Debug.Log("Чекпоинт сохранён: " + position);
    }

    public void Respawn()
    {
        Debug.Log("Респаун на: " + _lastCheckpoint);
        vrPlayer.position = _lastCheckpoint;

        Rigidbody rb = vrPlayer.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }
}
