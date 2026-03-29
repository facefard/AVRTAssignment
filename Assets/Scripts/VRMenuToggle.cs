using UnityEngine;
using UnityEngine.XR;

public class VRMenuToggle : MonoBehaviour
{
    public GameObject vrMenuCanvas;
    public Transform headCamera;

    public float distanceFromPlayer = 1.5f;
    public float heightOffset = -0.1f;

    private bool lastButtonState = false;

    void Start()
    {
        if (vrMenuCanvas != null)
        {
            vrMenuCanvas.SetActive(false);
        }
    }

    void Update()
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool buttonPressed = false;

        if (rightHand.isValid)
        {
            // This line controls the menu to open by RIGHT VR controller
            rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed);
        }

        if (buttonPressed && !lastButtonState)
        {
            ToggleMenu();
        }

        lastButtonState = buttonPressed;
    }

    public void ToggleMenu()
    {
        if (vrMenuCanvas == null || headCamera == null)
            return;

        bool willShow = !vrMenuCanvas.activeSelf;

        if (willShow)
        {
            Vector3 forward = headCamera.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 menuPosition = headCamera.position + forward * distanceFromPlayer;
            menuPosition.y += heightOffset;

            vrMenuCanvas.transform.position = menuPosition;

            vrMenuCanvas.transform.LookAt(headCamera);
            vrMenuCanvas.transform.Rotate(0f, 180f, 0f);

            vrMenuCanvas.SetActive(true);
        }
        else
        {
            vrMenuCanvas.SetActive(false);
        }
    }
}