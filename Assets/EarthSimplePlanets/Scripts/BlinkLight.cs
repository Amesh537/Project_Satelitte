using UnityEngine;

public class VRBlinkLight : MonoBehaviour
{
    public float speed = 3f;
    public float minEmission = 0f;
    public float maxEmission = 6f;

    public Color redColor = Color.red;
    public Color greenColor = Color.green;
    public GameObject miniLight;

    private Color currentColor;
    private Material mat;

    

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");

        currentColor = redColor; // start red
    }

    void Update()
    {
        float pulse = Mathf.Lerp(minEmission, maxEmission,
            (Mathf.Sin(Time.time * speed) + 1f) / 2f);

        mat.SetColor("_EmissionColor", currentColor * pulse);
    }

    // ✅ CALL THIS when puzzle is complete
    public void SetGreen()
    {
        currentColor = greenColor;
        
        if (miniLight != null)
            miniLight.SetActive(false);
    }

    // (optional) reset
    public void SetRed()
    {
        currentColor = redColor;
    }
}