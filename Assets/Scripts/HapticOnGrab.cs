using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticOnGrab : MonoBehaviour
{
    [Range(0f, 1f)] public float grabIntensity = 0.5f;

    private ContinuousHaptics activeHaptics;

    // Called when object is grabbed
    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Get the interactor (controller)
        var interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (interactor == null) return;

        // Look for ContinuousHaptics on the controller
        activeHaptics = interactor.GetComponent<ContinuousHaptics>();

        // If not found directly, try parent (common setup)
        if (activeHaptics == null)
            activeHaptics = interactor.GetComponentInParent<ContinuousHaptics>();

        if (activeHaptics != null)
        {
            activeHaptics.StartHapticsWithIntensity(grabIntensity);
        }
    }

    // Called when object is released
    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (activeHaptics != null)
        {
            activeHaptics.StopHaptics();
            activeHaptics = null;
        }
    }
}