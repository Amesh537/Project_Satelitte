using UnityEngine;

public class BodyRelativeFacingPanel : MonoBehaviour
{
    public Transform xrOrigin;   // XR Origin root
    public Transform head;       // Main Camera / headset

    [Header("Behavior")]
    public bool followRotationOfRig = false;   // Usually false for your case
    public bool faceHeadset = true;
    public bool yawOnly = true;                // Keeps panel upright

    private Vector3 localOffset;
    private Quaternion localRotationOffset;
    private bool pinned = false;

    public void PinHere()
    {
        if (xrOrigin == null) return;

        localOffset = xrOrigin.InverseTransformPoint(transform.position);
        localRotationOffset = Quaternion.Inverse(xrOrigin.rotation) * transform.rotation;
        pinned = true;
    }

    public void Unpin()
    {
        pinned = false;
    }

    void LateUpdate()
    {
        if (!pinned || xrOrigin == null) return;

        // Follow player movement
        transform.position = xrOrigin.TransformPoint(localOffset);

        // Optional: also follow rig turning
        if (followRotationOfRig)
            transform.rotation = xrOrigin.rotation * localRotationOffset;

        // Face the player's head
        if (faceHeadset && head != null)
        {
            Vector3 toHead = head.position - transform.position;

            if (yawOnly)
            {
                toHead.y = 0f;
                if (toHead.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(toHead.normalized, Vector3.up);
            }
            else
            {
                if (toHead.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(toHead.normalized, Vector3.up);
            }
        }
    }
}