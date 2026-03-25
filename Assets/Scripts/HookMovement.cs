using UnityEngine;

public class HookMovement : MonoBehaviour
{
    [Header("Settings")]
    public Rigidbody playerRigidbody;
    public float pullStrength = 50f;
    public HookMovement otherHook;

    private Vector3 _hookedWorldPoint;
    private bool _isHooked = false;

    public bool IsHooked => _isHooked;

    void FixedUpdate()
    {
        
        if (playerRigidbody.linearVelocity.magnitude > 5f)
        {
            playerRigidbody.linearVelocity = playerRigidbody.linearVelocity.normalized * 5f;
        }

        if (_isHooked)
        {
            Vector3 handToHook = _hookedWorldPoint - transform.position;
            playerRigidbody.AddForce(handToHook * pullStrength, ForceMode.Acceleration);
            playerRigidbody.linearVelocity *= 0.85f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Hookable")) return;

        // крюк должен быть выше точки касания
        if (transform.position.y > collision.contacts[0].point.y)
        {
            _hookedWorldPoint = collision.contacts[0].point;
            _isHooked = true;

            if (otherHook != null && otherHook.IsHooked)
                otherHook.ForceDetach();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.collider.CompareTag("Hookable")) return;
        if (!_isHooked) return;
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.collider.CompareTag("Hookable")) return;
        _isHooked = false;
    }

    public void ForceDetach()
    {
        _isHooked = false;
    }
}