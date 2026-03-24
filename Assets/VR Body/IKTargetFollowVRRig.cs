using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;      // XR device (camera/controller)
    public Transform ikTarget;      // IK target on avatar

    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        if (vrTarget == null || ikTarget == null) return;

        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class IKTargetFollowVRRig : MonoBehaviour
{
    [Header("Smoothing")]
    [Range(0f, 1f)]
    public float positionSmoothness = 0.15f;

    [Range(0f, 1f)]
    public float rotationSmoothness = 0.1f;

    [Header("VR Targets")]
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    [Header("Body Offsets")]
    public Vector3 headBodyPositionOffset;
    public float headBodyYawOffset;

    void LateUpdate()
    {
        if (head.vrTarget == null) return;

        // 1️⃣ Update IK targets FIRST (prevents feedback loop)
        head.Map();
        leftHand.Map();
        rightHand.Map();

        // 2️⃣ Follow headset position (FULL 3D for flying)
        Vector3 targetPosition = head.vrTarget.position + headBodyPositionOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            positionSmoothness
        );

        // 3️⃣ Rotate body only on Y axis (prevents weird tilting)
        float targetYaw = head.vrTarget.eulerAngles.y + headBodyYawOffset;

        Quaternion targetRotation = Quaternion.Euler(0, targetYaw, 0);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSmoothness
        );
    }
}