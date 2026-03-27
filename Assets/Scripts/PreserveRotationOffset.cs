using UnityEngine;

public class PreserveRotationOffset : MonoBehaviour
{
    private Quaternion _sceneRotation;

    void Awake()
    {
        _sceneRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.localRotation = _sceneRotation * transform.localRotation;
    }
}
