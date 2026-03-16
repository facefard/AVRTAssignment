using UnityEngine;

public class HookMovement : MonoBehaviour
{
    [Header("Settings")]
    public Rigidbody playerRigidbody;
    public float pullStrength = 50f;
    public HookMovement otherHook;

    private Vector3 _hookedWorldPoint;
    private Vector3 _previousHandPos;
    private bool _isHooked = false;

    public bool IsHooked => _isHooked;

    void Start()
    {
        _previousHandPos = transform.position;
    }

    void FixedUpdate()
    {
        if (_isHooked)
        {
            Vector3 handToHook = _hookedWorldPoint - transform.position;
            playerRigidbody.AddForce(handToHook * pullStrength, ForceMode.Acceleration);
        }

        CheckHook();
    }

    void CheckHook()
    {
        Vector3 currentPos = transform.position;
        Vector3 delta = currentPos - _previousHandPos;
        float distance = delta.magnitude;

        bool found = false;

        // проверяем 10 точек между прошлой и текущей позицией
        int steps = Mathf.Max(1, Mathf.CeilToInt(distance / 0.01f));
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            Vector3 checkPos = Vector3.Lerp(_previousHandPos, currentPos, t);

            Collider[] hits = Physics.OverlapSphere(checkPos, 0.05f);
            foreach (Collider col in hits)
            {
                if (col.GetComponent<Hookable>() != null)
                {
                    if (!_isHooked)
                    {
                        _hookedWorldPoint = checkPos;

                        if (otherHook != null && otherHook.IsHooked)
                            otherHook.ForceDetach();
                    }
                    found = true;
                    break;
                }
            }
            if (found) break;
        }

        _isHooked = found;
        _previousHandPos = currentPos;
    }

    public void ForceDetach()
    {
        _isHooked = false;
    }
}