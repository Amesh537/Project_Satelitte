using UnityEngine;

public class WirePort : MonoBehaviour
{
    public int portID;

    public Renderer portRenderer;
    public Color defaultColor = Color.white;
    public Color correctColor = Color.green;

    void Start()
    {
        if (portRenderer != null)
            portRenderer.material.color = defaultColor;
    }

    public void SetCorrect()
    {
        if (portRenderer != null)
            portRenderer.material.color = correctColor;
    }
}