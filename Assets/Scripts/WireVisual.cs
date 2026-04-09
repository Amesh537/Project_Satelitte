using UnityEngine;


public class WireVisual : MonoBehaviour
{
    public Transform startPoint;   // Left side (fixed)
    public Transform endPoint;     // The grabbable end
    public LineRenderer line;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    public float maxLength = 1.2f; // Optional limit

    void Update()
    {
        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);
    }
}