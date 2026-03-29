using UnityEngine;
using UnityEngine.XR;

public class VRMenuToggle : MonoBehaviour
{
    [Header("References")]
    public GameObject vrMenuCanvas;  
    public Transform headCamera;      
    [Header("Menu Position")]
    public float distanceFromPlayer = 0.3f;
    public float heightOffset = 0f;

    [Header("Controller")]
    public XRNode controllerNode = XRNode.RightHand;

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
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        bool buttonPressed = false;

        if (device.isValid)
        {
            // VR controller button
            device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed);
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

       
            vrMenuCanvas.transform.rotation =
                Quaternion.Euler(0f, headCamera.eulerAngles.y + 180f, 0f);

            vrMenuCanvas.SetActive(true);
        }
        else
        {
            vrMenuCanvas.SetActive(false);
        }
    }
}