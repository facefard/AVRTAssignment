using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HookMovement : MonoBehaviour
{
    [Header("Settings")]
    public Rigidbody playerRigidbody;
    public float pullStrength = 50f;
    public float maxSpeed = 5f;
    public float detachSpeed = 2f; // скорость руки для отцепа
    public float detachDelay = 0.1f; // задержка отцепа в секундах
    public float jumpHeight = 5f; // фиксированная высота прыжка
    public float horizontalBoost = 3f; // фиксированная горизонтальная скорость
    public HookMovement otherHook;

    [Header("Smoothing")]
    public int speedBuffer = 5; // усреднение скорости за N кадров

    private Vector3 _hookedWorldPoint;
    private Vector3 _previousHookTipPos;
    private bool _isHooked = false;

    private Queue<float> _handSpeeds = new Queue<float>();
    private float _detachTimer = 0f;

    public bool IsHooked => _isHooked;

    void FixedUpdate()
    {
        // Ограничение скорости игрока
        if (playerRigidbody.linearVelocity.magnitude > maxSpeed)
            playerRigidbody.linearVelocity = playerRigidbody.linearVelocity.normalized * maxSpeed;

        Vector3 direction = transform.position - _previousHookTipPos;
        float distance = direction.magnitude;

        // -----------------------------
        // Проверка отцепа при высокой скорости
        // -----------------------------
        if (_isHooked)
        {
            float handSpeed = direction.magnitude / Time.fixedDeltaTime;

            // усреднение скорости
            _handSpeeds.Enqueue(handSpeed);
            if (_handSpeeds.Count > speedBuffer)
                _handSpeeds.Dequeue();
            float avgSpeed = _handSpeeds.Average();

            if (avgSpeed >= detachSpeed)
            {
                _detachTimer += Time.fixedDeltaTime;

                if (_detachTimer >= detachDelay)
                {
                    ForceDetach();

                    // Диагональный прыжок
                    float jumpVelocityY = Mathf.Sqrt(2f * 9.81f * jumpHeight);
                    Vector3 horizontalDir = new Vector3(playerRigidbody.transform.forward.x, 0f, playerRigidbody.transform.forward.z).normalized;
                    Vector3 jumpVelocity = horizontalDir * horizontalBoost;
                    jumpVelocity.y = jumpVelocityY;

                    playerRigidbody.linearVelocity = jumpVelocity;

                    _detachTimer = 0f;
                }
            }
            else
            {
                _detachTimer = 0f; // сброс таймера, если скорость меньше detachSpeed
            }
        }

        // -----------------------------
        // Проверка зацепления
        // -----------------------------
        if (!_isHooked && distance > 0.001f)
        {
            RaycastHit hit;
            if (Physics.SphereCast(_previousHookTipPos, 0.03f, direction.normalized, out hit, distance))
            {
                if (hit.collider.CompareTag("Hookable") &&
                    _previousHookTipPos.y > hit.point.y)
                {
                    _hookedWorldPoint = hit.point;
                    _isHooked = true;

                    if (otherHook != null && otherHook.IsHooked)
                        otherHook.ForceDetach();

                    HookHitAudio.RequestPlay();
                }
            }
        }

        // -----------------------------
        // Тянем игрока к точке крюка
        // -----------------------------
        if (_isHooked)
        {
            Vector3 hookDrift = transform.position - _hookedWorldPoint;
            playerRigidbody.MovePosition(playerRigidbody.position - hookDrift);
            playerRigidbody.linearVelocity *= 0.85f; // плавное затухание
        }

        _previousHookTipPos = transform.position;
    }

    public void ForceDetach()
    {
        _isHooked = false;
        _handSpeeds.Clear();
        _detachTimer = 0f;
    }
}