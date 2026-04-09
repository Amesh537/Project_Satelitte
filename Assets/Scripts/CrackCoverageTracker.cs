using UnityEngine;
using UnityEngine.Events;

public class CrackCoverageTracker : MonoBehaviour
{
    [System.Serializable]
    public class CrackSegment
    {
        public Transform point;

        [Header("Radius")]
        public bool useCustomRadius = false;
        public float customRadius = 0.015f;

        [HideInInspector] public bool covered;
    }

    [Header("Global Settings")]
    public float globalRadius = 0.015f;

    public CrackSegment[] segments;
    public GameObject miniLight;

    [SerializeField] private ObjectiveItem objectiveToComplete;

    [Header("Completion")]
    public int requiredCoveredSegments = 0;

    private bool isComplete = false;

    void Start()
    {
        if (requiredCoveredSegments <= 0)
            requiredCoveredSegments = segments.Length;
    }

    public void RegisterWeldPoint(Vector3 worldPoint)
    {
        if (isComplete || segments == null || segments.Length == 0)
            return;

        float bestDistance = float.MaxValue;
        int bestIndex = -1;

        for (int i = 0; i < segments.Length; i++)
        {
            float radius = segments[i].useCustomRadius 
                ? segments[i].customRadius 
                : globalRadius;

            float d = Vector3.Distance(worldPoint, segments[i].point.position);

            if (d <= radius && d < bestDistance)
            {
                bestDistance = d;
                bestIndex = i;
            }
        }

        if (bestIndex >= 0)
        {
            segments[bestIndex].covered = true;
            CheckCompletion();
        }
    }

    public int GetCoveredCount()
    {
        int count = 0;
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].covered)
                count++;
        }
        return count;
    }

    void CheckCompletion()
    {
        int covered = GetCoveredCount();

        if (!isComplete && covered >= requiredCoveredSegments)
        {
            isComplete = true;
            objectiveToComplete.CompleteObjective();

            Debug.Log("Crack welding complete.");
            if (miniLight != null)
            {
                miniLight.SetActive(false);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (segments == null) return;

        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].point == null) continue;

            float radius = segments[i].useCustomRadius 
                ? segments[i].customRadius 
                : globalRadius;

            Gizmos.color = segments[i].covered ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(segments[i].point.position, radius);
        }
    }
}