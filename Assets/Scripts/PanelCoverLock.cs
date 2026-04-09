using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanelCoverLock : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable[] switches; // assign all switches, knobs, etc.

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Start()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // ❌ disable interaction (NOT visibility)
        SetSwitchesEnabled(false);

        grab.selectExited.AddListener(OnRemoved);
    }

    void OnRemoved(SelectExitEventArgs args)
    {
        // ✅ enable interaction when cover removed
        SetSwitchesEnabled(true);
    }

    void SetSwitchesEnabled(bool enabled)
    {
        foreach (var s in switches)
        {
            s.enabled = enabled;
        }
    }
}