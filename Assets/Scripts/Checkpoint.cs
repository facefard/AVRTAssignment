using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Settings")]
    public Transform spawnPoint;

    [Header("Animation")]
    public Animator animator; // добавили

    private bool _isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (_isActivated) return;

        Debug.Log("Кто-то вошёл в триггер: " + other.name);

        if (other.GetComponent<CheckpointTrigger>() == null) return;

        Debug.Log("Чекпоинт активирован!");

        _isActivated = true;

        // 👉 запускаем анимацию
        if (animator != null)
        {
            animator.SetTrigger("Activate");
        }

        CheckpointManager.Instance.SetCheckpoint(spawnPoint.position);
    }
}