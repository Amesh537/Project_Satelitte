using UnityEngine;

public class WeldBrushPainter : MonoBehaviour
{
    [Header("References")]
    public TriggerEffectToggle torchController;
    public Transform torchTip;
    public GameObject weldBlobPrefab;

    [Header("Raycast")]
    public LayerMask crackLayer;
    public float rayDistance = 0.08f;
    public float surfaceOffset = 0.0015f;

    [Header("Brush Settings")]
    public float blobSpacing = 0.008f;
    public bool parentBlobsToSurface = true;

    private Vector3 lastSpawnPoint;
    private Vector3 lastSpawnNormal;
    private Transform lastHitTransform;
    private bool hasLastPoint = false;

    public CrackCoverageTracker crackCoverageTracker;

    void Update()
    {
        if (torchController == null || torchTip == null || weldBlobPrefab == null)
            return;

        if (!torchController.IsTorchOn)
        {
            hasLastPoint = false;
            lastHitTransform = null;
            return;
        }

        Ray ray = new Ray(torchTip.position, torchTip.forward);
        Debug.DrawRay(torchTip.position, torchTip.forward * rayDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, crackLayer))
        {
            Vector3 currentPoint = hit.point + hit.normal * surfaceOffset;

            if (!hasLastPoint || hit.collider.transform != lastHitTransform)
            {
                PlaceBlob(currentPoint, hit.normal, hit.collider.transform);
                lastSpawnPoint = currentPoint;
                lastSpawnNormal = hit.normal;
                lastHitTransform = hit.collider.transform;
                hasLastPoint = true;
                return;
            }

            float dist = Vector3.Distance(lastSpawnPoint, currentPoint);

            if (dist >= blobSpacing)
            {
                int steps = Mathf.FloorToInt(dist / blobSpacing);

                for (int i = 1; i <= steps; i++)
                {
                    float t = (float)i / steps;
                    Vector3 p = Vector3.Lerp(lastSpawnPoint, currentPoint, t);
                    Vector3 n = Vector3.Slerp(lastSpawnNormal, hit.normal, t).normalized;

                    PlaceBlob(p, n, hit.collider.transform);
                }

                lastSpawnPoint = currentPoint;
                lastSpawnNormal = hit.normal;
                lastHitTransform = hit.collider.transform;
            }
        }
        else
        {
            hasLastPoint = false;
            lastHitTransform = null;
        }
    }
    
    void PlaceBlob(Vector3 position, Vector3 normal, Transform hitTransform)
    {
        Quaternion rotation = hitTransform.rotation;

        GameObject blob = Instantiate(weldBlobPrefab, position, rotation);

        if (parentBlobsToSurface && hitTransform != null)
            blob.transform.SetParent(hitTransform, true);

        if (crackCoverageTracker != null)
            crackCoverageTracker.RegisterWeldPoint(position);
    }
}