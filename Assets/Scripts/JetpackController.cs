using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class JetpackController : MonoBehaviour
{
    public Rigidbody rb;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform xrOrigin;

    public InputActionProperty rightTrigger;
    public InputActionProperty leftStick;
    public InputActionProperty leftBackward;

    [Header("Rotation")]
    public InputActionProperty rightStick;
    public float rotationSpeed = 60f;

    private float smoothTurnInput = 0f;
    public float turnSmoothSpeed = 5f;

    [Header("Boost")]
    public InputActionProperty boostButton;
    public float boostMultiplier = 2f;

    [Header("Brake")]
    public InputActionProperty brakeButton;
    public float brakeStrength = 8f;

    public float thrustPower = 10f;
    public float verticalPower = 8f;
    public float maxSpeed = 6f;

    public AudioSource jetpackAudio;
    public HapticImpulsePlayer leftHaptics;
    public HapticImpulsePlayer rightHaptics;
    public AudioSource leftThrusterAudio;
    public AudioSource rightThrusterAudio;

    [Header("Haptics")]
    public float hapticStrength = 0.3f;
    public float hapticDuration = 0.1f;
    public float hapticInterval = 0.15f;

    private float hapticTimer = 0f;

    [Header("Directional Haptics")]
    public float directionalHapticStrength = 0.3f;
    public float directionalHapticDuration = 0.08f;
    public float directionalHapticInterval = 0.12f;

    private float leftHapticTimer = 0f;
    private float rightHapticTimer = 0f;
    
    void Update()
    {
        Vector2 rot = rightStick.action.ReadValue<Vector2>();

        float deadzone = 0.2f;
        float target = 0f;

        if (Mathf.Abs(rot.x) > deadzone)
            target = rot.x;

        // Smooth the input
        smoothTurnInput = Mathf.Lerp(smoothTurnInput, target, Time.deltaTime * turnSmoothSpeed);

        xrOrigin.Rotate(Vector3.up, smoothTurnInput * rotationSpeed * Time.deltaTime, Space.World);
    }

    void FixedUpdate()
    {
        float boost = boostButton.action.IsPressed() ? boostMultiplier : 1f;
        bool isBoosting = boostButton.action.IsPressed();

        if (rightTrigger.action.IsPressed())
            rb.AddForce(head.forward * thrustPower * boost, ForceMode.Acceleration);

        Vector2 stick = leftStick.action.ReadValue<Vector2>();
        if (stick.x < -0.1f)
        {
            if (!leftThrusterAudio.isPlaying)
                leftThrusterAudio.Play();

            leftHapticTimer += Time.fixedDeltaTime;

            if (leftHapticTimer >= directionalHapticInterval)
            {
                leftHapticTimer = 0f;

                if (leftHaptics != null)
                    leftHaptics.SendHapticImpulse(directionalHapticStrength, directionalHapticDuration);
            }
        }
        else
        {
            if (leftThrusterAudio.isPlaying)
                leftThrusterAudio.Stop();

            leftHapticTimer = 0f;
        }

        if (stick.x > 0.1f)
        {
            if (!rightThrusterAudio.isPlaying)
                rightThrusterAudio.Play();

            rightHapticTimer += Time.fixedDeltaTime;

            if (rightHapticTimer >= directionalHapticInterval)
            {
                rightHapticTimer = 0f;

                if (rightHaptics != null)
                    rightHaptics.SendHapticImpulse(directionalHapticStrength, directionalHapticDuration);
            }
        }
        else
        {
            if (rightThrusterAudio.isPlaying)
                rightThrusterAudio.Stop();

            rightHapticTimer = 0f;
        }

        if (Mathf.Abs(stick.y) > 0.1f)
            rb.AddForce(Vector3.up * stick.y * verticalPower, ForceMode.Acceleration);

        if (Mathf.Abs(stick.x) > 0.1f)
            rb.AddForce(head.right * stick.x * verticalPower, ForceMode.Acceleration);

        if (leftBackward.action.IsPressed())
            rb.AddForce(-leftHand.forward * thrustPower, ForceMode.Acceleration);

        if (brakeButton.action.IsPressed())
            rb.AddForce(-rb.linearVelocity * brakeStrength, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        if (isBoosting)
        {
            if (!jetpackAudio.isPlaying)
                jetpackAudio.Play();
        }
        else
        {
            if (jetpackAudio.isPlaying)
                jetpackAudio.Stop();
        }

        if (isBoosting)
        {
            hapticTimer += Time.fixedDeltaTime;

            if (hapticTimer >= hapticInterval)
            {
                hapticTimer = 0f;

                if (leftHaptics != null)
                    leftHaptics.SendHapticImpulse(hapticStrength, hapticDuration);

                if (rightHaptics != null)
                    rightHaptics.SendHapticImpulse(hapticStrength, hapticDuration);
            }
        }
        else
        {
            hapticTimer = 0f;
        }
    }

}
