using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class BodyHookLatch : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform homeAnchor;
    [SerializeField] private GameObject locomotionObject;
    [SerializeField] private Transform playerRoot;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private bool isGrabbed;
    private bool isLatched;

    private HookLatchPoint currentLatchCandidate;
    private HookLatchPoint latchedPoint;

    private bool isAligningPlayer;

    private void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void Start()
    {
        MoveHome();
        SetLocomotionEnabled(true);
    }

    private void LateUpdate()
    {
        if (!isGrabbed)
        {
            if (isLatched && latchedPoint != null)
            {
                transform.position = latchedPoint.SnapPoint.position;
                transform.rotation = latchedPoint.SnapPoint.rotation;
            }
            else if (homeAnchor != null)
            {
                transform.position = homeAnchor.position;
                transform.rotation = homeAnchor.rotation;
            }
        }

        if (isAligningPlayer && latchedPoint != null)
        {
            AlignPlayerToLatchedPoint();
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        // Grabbing a latched hook immediately unlatches it
        if (isLatched)
        {
            UnlatchAndReturnControl();
        }

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;

        if (currentLatchCandidate != null)
        {
            LatchTo(currentLatchCandidate);
        }
        else
        {
            MoveHome();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HookLatchPoint latchPoint = other.GetComponent<HookLatchPoint>();
        if (latchPoint != null)
        {
            currentLatchCandidate = latchPoint;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        HookLatchPoint latchPoint = other.GetComponent<HookLatchPoint>();
        if (latchPoint != null && currentLatchCandidate == latchPoint)
        {
            currentLatchCandidate = null;
        }
    }

    private void LatchTo(HookLatchPoint latchPoint)
    {
        isLatched = true;
        latchedPoint = latchPoint;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        transform.position = latchPoint.SnapPoint.position;
        transform.rotation = latchPoint.SnapPoint.rotation;

        SetLocomotionEnabled(false);

        if (latchedPoint.AlignPlayerOnLatch && latchedPoint.PlayerAlignPoint != null && playerRoot != null)
        {
            isAligningPlayer = true;
        }
    }

    private void UnlatchAndReturnControl()
    {
        isLatched = false;
        isAligningPlayer = false;
        latchedPoint = null;

        SetLocomotionEnabled(true);
    }

    private void MoveHome()
    {
        isLatched = false;
        isAligningPlayer = false;
        latchedPoint = null;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (homeAnchor != null)
        {
            transform.position = homeAnchor.position;
            transform.rotation = homeAnchor.rotation;
        }

        SetLocomotionEnabled(true);
    }

    private void SetLocomotionEnabled(bool enabled)
    {
        if (locomotionObject != null)
        {
            locomotionObject.SetActive(enabled);
        }
    }

    private void AlignPlayerToLatchedPoint()
    {
        if (latchedPoint == null || latchedPoint.PlayerAlignPoint == null || playerRoot == null)
        {
            isAligningPlayer = false;
            return;
        }

        Transform target = latchedPoint.PlayerAlignPoint;

        playerRoot.position = Vector3.MoveTowards(
            playerRoot.position,
            target.position,
            latchedPoint.AlignMoveSpeed * Time.deltaTime);

        float posError = Vector3.Distance(playerRoot.position, target.position);

        if (posError < 0.01f)
        {
            playerRoot.position = target.position;
            isAligningPlayer = false;
        }
    }
}