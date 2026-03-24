using UnityEngine;

public class WheelRotationCheck : MonoBehaviour
{
    public Transform wheelHandle;
    public SliderLight light;
    public int requiredRotations = 3;

    private float totalRotation = 0f;
    private float lastAngle;

    void Start()
    {
        lastAngle = wheelHandle.localEulerAngles.y;
    }

    void Update()
    {
        float currentAngle = wheelHandle.localEulerAngles.y;

        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
        lastAngle = currentAngle;

        // ✅ Clockwise only (positive rotation)
        if (delta > 0)
        {
            totalRotation += delta;
        }
        else if (delta < -1f) // small buffer to avoid noise
        {
            // ❌ Any counterclockwise movement resets progress
            totalRotation = 0f;
        }

        int rotations = Mathf.FloorToInt(totalRotation / 360f);

        bool isCorrect = rotations >= requiredRotations;
        light.SetCorrect(isCorrect);
    }
}