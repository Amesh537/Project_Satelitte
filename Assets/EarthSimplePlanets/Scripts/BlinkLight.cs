using UnityEngine;

public class VRBlinkLight : MonoBehaviour
{
    public float speed = 3f;
    public float minEmission = 0f;
    public float maxEmission = 6f;

    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    public GameObject miniLight;

    public SliderLight leverLight; // 👈 ADD THIS

    private Color currentColor;
    private Material mat;
    private bool isGreen = false;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");

        currentColor = redColor; // start red
    }

    void Update()
    {
        // 👇 CHECK lever light instead of being told directly
        if (!isGreen && leverLight != null && leverLight.IsCorrect())
        {
            SetGreen();
        }

        float pulse = Mathf.Lerp(minEmission, maxEmission,
            (Mathf.Sin(Time.time * speed) + 1f) / 2f);

        mat.SetColor("_EmissionColor", currentColor * pulse);
    }

    public void SetGreen()
    {
        currentColor = greenColor;
        isGreen = true;

        if (miniLight != null)
            miniLight.SetActive(false);
    }

    public void SetRed()
    {
        currentColor = redColor;
        isGreen = false;
    }
}