using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour

{
public GameObject menu;
public GameObject RayInteractor;
public InputActionProperty showButton;

public Transform head;  
public float spawnDistance = 2f; 
    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            RayInteractor.SetActive(!RayInteractor.activeSelf);
            menu.SetActive(!menu.activeSelf);

            if (menu.activeSelf)
            {
                Vector3 direction = new Vector3(head.forward.x, 0, head.forward.z).normalized;
                menu.transform.position = head.position + direction * spawnDistance;
                menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
                menu.transform.forward *= -1;
            }
        }
    }
}