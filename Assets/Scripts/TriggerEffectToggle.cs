using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class TriggerEffectToggle : MonoBehaviour
{
    public enum HandToUse
    {
        Left,
        Right
    }

    [Header("Input")]
    public HandToUse handToUse = HandToUse.Right;
    public float triggerThreshold = 0.1f;

    [Header("Visuals")]
    public GameObject effectObject;

    [Header("Audio")]
    public AudioSource startAudio;
    public AudioSource loopAudio;
    public AudioSource stopAudio;

    [Header("Haptics")]
    public ContinuousHaptics continuousHaptics;

    private InputDevice device;
    private bool isEffectOn = false;
    private Coroutine loopRoutine;

    public bool IsTorchOn => isEffectOn;

    void Start()
    {
        RefreshDevice();

        if (effectObject != null)
            effectObject.SetActive(false);

        if (loopAudio != null)
            loopAudio.loop = true;
    }

    void Update()
    {
        if (!device.isValid)
            RefreshDevice();

        float triggerValue = 0f;
        device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);

        bool triggerPressed = triggerValue > triggerThreshold;

        if (triggerPressed)
        {
            if (!isEffectOn)
                StartEffect();
        }
        else
        {
            if (isEffectOn)
                StopEffect();
        }
    }

    void RefreshDevice()
    {
        XRNode node = handToUse == HandToUse.Left ? XRNode.LeftHand : XRNode.RightHand;
        device = InputDevices.GetDeviceAtXRNode(node);
    }

    void StartEffect()
    {
        isEffectOn = true;

        if (effectObject != null)
            effectObject.SetActive(true);

        if (stopAudio != null && stopAudio.isPlaying)
            stopAudio.Stop();

        if (startAudio != null)
            startAudio.Play();

        if (loopRoutine != null)
            StopCoroutine(loopRoutine);

        float delay = 0f;
        if (startAudio != null && startAudio.clip != null)
            delay = startAudio.clip.length;

        loopRoutine = StartCoroutine(StartLoopAfterDelay(delay));

        if (continuousHaptics != null)
            continuousHaptics.StartHaptics();
    }

    IEnumerator StartLoopAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isEffectOn && loopAudio != null)
            loopAudio.Play();
    }

    void StopEffect()
    {
        isEffectOn = false;

        if (effectObject != null)
            effectObject.SetActive(false);

        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }

        if (startAudio != null && startAudio.isPlaying)
            startAudio.Stop();

        if (loopAudio != null && loopAudio.isPlaying)
            loopAudio.Stop();

        if (stopAudio != null)
            stopAudio.Play();

        if (continuousHaptics != null)
            continuousHaptics.StopHaptics();
    }

    void OnDisable()
    {
        StopEffect();
    }
}