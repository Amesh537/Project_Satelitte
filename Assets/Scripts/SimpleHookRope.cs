using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SimpleHookRope : MonoBehaviour
{
    [SerializeField] private Transform ropeStart;
    [SerializeField] private Transform hookEnd;

    [Header("Optional slack")]
    [SerializeField] private bool addMidPointSag = false;
    [SerializeField] private float sagAmount = 0.03f;

    private LineRenderer line;

    private readonly Vector3[] twoPoints = new Vector3[2];
    private readonly Vector3[] threePoints = new Vector3[3];

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (ropeStart == null || hookEnd == null)
            return;

        if (!addMidPointSag)
        {
            if (line.positionCount != 2)
                line.positionCount = 2;

            twoPoints[0] = ropeStart.position;
            twoPoints[1] = hookEnd.position;

            line.SetPositions(twoPoints);
        }
        else
        {
            if (line.positionCount != 3)
                line.positionCount = 3;

            Vector3 start = ropeStart.position;
            Vector3 end = hookEnd.position;
            Vector3 mid = (start + end) * 0.5f + Vector3.down * sagAmount;

            threePoints[0] = start;
            threePoints[1] = mid;
            threePoints[2] = end;

            line.SetPositions(threePoints);
        }
    }
}