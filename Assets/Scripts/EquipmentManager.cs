using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject minimap;
    public GameObject weldingTorch;

    public void Equip(int index)
    {
        // Turn OFF all tools first
        flashlight.SetActive(false);
        minimap.SetActive(false);
        weldingTorch.SetActive(false);

        // Turn ON selected tool (if not hand)
        switch (index)
        {
            case 0: // Flashlight
                flashlight.SetActive(true);
                break;

            case 1: // Minimap
                minimap.SetActive(true);
                break;

            case 2: // Welding Torch
                weldingTorch.SetActive(true);
                break;

            case 3: // Hand (do nothing, all tools already off)
                break;
        }
    }
}