using UnityEngine;

public class SliderLight : MonoBehaviour
{
    public Renderer rend;
    public Material redMat;
    public Material greenMat;

    public float correctMin = 0.8f;
    public float correctMax = 1.0f;

    public bool isCorrect = false;

    void Start()
    {
        SetCorrect(false); // start red
    }

    public void CheckSlider(float value)
    {
        bool correct = value >= correctMin && value <= correctMax;
        SetCorrect(correct);
    }

    public void SetCorrect(bool correct)
    {
        isCorrect = correct;
        rend.material = isCorrect ? greenMat : redMat;
    }

    // ✅ ADD THIS FUNCTION
    public bool IsCorrect()
    {
        return isCorrect;
    }
}