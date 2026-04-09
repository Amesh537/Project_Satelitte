using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class WireConnection : MonoBehaviour
{
    public int wireID;

    public AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip zapSound;
    public WirePuzzleManager puzzleManager;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private HapticImpulsePlayer currentHaptics; // 👈 dynamic hand
    private bool isConnected = false;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // Get the hand/controller that grabbed this
        var interactorObject = args.interactorObject.transform;

        // Try to find HapticImpulsePlayer on that controller
        currentHaptics = interactorObject.GetComponentInParent<HapticImpulsePlayer>();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (isConnected) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (var hit in hits)
        {
            WirePort port = hit.GetComponent<WirePort>();

            if (port != null)
            {
                if (port.portID == wireID)
                {
                    ConnectCorrect(port);
                }
                else
                {
                    ConnectWrong();
                }
                return;
            }
        }
    }

    void ConnectCorrect(WirePort port)
    {
        isConnected = true;

        transform.position = port.transform.position;
        transform.SetParent(port.transform);

        grab.enabled = false;

        port.SetCorrect();

        if (currentHaptics != null)
            currentHaptics.SendHapticImpulse(0.5f, 0.2f);

        if (audioSource != null && successSound != null)
            audioSource.PlayOneShot(successSound);

        // ✅ NEW
        if (puzzleManager != null)
            puzzleManager.RegisterCorrectConnection();

        Debug.Log("Correct Connection!");
    }

    void ConnectWrong()
    {
        // ❌ Strong haptics (correct hand)
        if (currentHaptics != null)
            currentHaptics.SendHapticImpulse(1.0f, 0.3f);

        // ❌ Zap sound
        if (audioSource != null && zapSound != null)
            audioSource.PlayOneShot(zapSound);

        // Bounce back
        transform.position += Random.insideUnitSphere * 0.05f;

        Debug.Log("Wrong Connection!");
    }
}