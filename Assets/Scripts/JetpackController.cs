using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Boost")]
    public InputActionProperty boostButton;
    public float boostMultiplier = 2f;

    [Header("Brake")]
    public InputActionProperty brakeButton;
    public float brakeStrength = 8f;

    public float thrustPower = 10f;
    public float verticalPower = 8f;
    public float maxSpeed = 6f;

    void Update()
    {
        Vector2 rot = rightStick.action.ReadValue<Vector2>();
        xrOrigin.Rotate(Vector3.up, rot.x * rotationSpeed * Time.deltaTime, Space.World);
    }

    void FixedUpdate()
    {
        float boost = boostButton.action.IsPressed() ? boostMultiplier : 1f;

        if (rightTrigger.action.IsPressed())
            rb.AddForce(head.forward * thrustPower * boost, ForceMode.Acceleration);

        Vector2 stick = leftStick.action.ReadValue<Vector2>();
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
    }

}
