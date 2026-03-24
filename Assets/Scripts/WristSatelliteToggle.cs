using UnityEngine;
using UnityEngine.InputSystem;

public class WristSatelliteToggle : MonoBehaviour
{
    public GameObject miniSatellite; // your wrist model
    public InputActionProperty toggleButton; // Y button

    private bool isVisible = false;

    void Update()
    {
        if (toggleButton.action.WasPressedThisFrame())
        {
            isVisible = !isVisible;
            miniSatellite.SetActive(isVisible);
        }
    }
}