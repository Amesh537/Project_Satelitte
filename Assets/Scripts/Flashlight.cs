using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlightLight;

    private bool isOn = false;

    void Start()
    {
        TurnOn();
    }

    public void TurnOn()
    {
        flashlightLight.enabled = true;
        isOn = true;
    }

    public void TurnOff()
    {
        flashlightLight.enabled = false;
        isOn = false;
    }

    public void Toggle()
    {
        if (isOn)
            TurnOff();
        else
            TurnOn();
    }
}