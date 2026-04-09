using UnityEngine;

public class WeldBlobCooling : MonoBehaviour
{
    public Renderer targetRenderer;
    public Color hotColor = new Color(1f, 0.45f, 0.1f);
    public Color coolColor = new Color(0.35f, 0.35f, 0.38f);
    public float coolTime = 2f;

    private Material runtimeMaterial;
    private float timer = 0f;

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
            runtimeMaterial = targetRenderer.material;
    }

    void Update()
    {
        if (runtimeMaterial == null)
            return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / coolTime);
        runtimeMaterial.color = Color.Lerp(hotColor, coolColor, t);
    }
}