using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class ContinuousHaptics : MonoBehaviour
{
    [Range(0f, 1f)] public float amplitude = 0.5f;
    public float pulseInterval = 0.1f;

    private HapticImpulsePlayer hapticPlayer;
    private Coroutine hapticCoroutine;

    void Awake()
    {
        hapticPlayer = GetComponent<HapticImpulsePlayer>();
    }

    public void StartHaptics()
    {
        if (hapticCoroutine != null)
            StopCoroutine(hapticCoroutine);

        hapticCoroutine = StartCoroutine(HapticLoop());
    }

    public void StopHaptics()
    {
        if (hapticCoroutine != null)
        {
            StopCoroutine(hapticCoroutine);
            hapticCoroutine = null;
        }
    }

    // Call this from XR Grab Interactable events
    public void StartHapticsWithIntensity(float newAmplitude)
    {
        amplitude = Mathf.Clamp01(newAmplitude);
        StartHaptics();
    }

    // Optional if you want to change intensity while already vibrating
    public void SetIntensity(float newAmplitude)
    {
        amplitude = Mathf.Clamp01(newAmplitude);
    }

    IEnumerator HapticLoop()
    {
        while (true)
        {
            hapticPlayer?.SendHapticImpulse(amplitude, pulseInterval);
            yield return new WaitForSeconds(pulseInterval);
        }
    }
}