using UnityEngine;

public class HookLatchPoint : MonoBehaviour
{
    [Header("Hook")]
    [SerializeField] private Transform snapPoint;

    [Header("Player Alignment")]
    [SerializeField] private Transform playerAlignPoint;
    [SerializeField] private bool alignPlayerOnLatch = true;
    [SerializeField] private float alignMoveSpeed = 1.5f;

    public Transform SnapPoint => snapPoint != null ? snapPoint : transform;
    public Transform PlayerAlignPoint => playerAlignPoint;
    public bool AlignPlayerOnLatch => alignPlayerOnLatch;
    public float AlignMoveSpeed => alignMoveSpeed;
}