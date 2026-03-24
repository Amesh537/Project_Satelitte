using UnityEngine;

public class LeverPuzzleCheck : MonoBehaviour
{
    public SliderLight leverLight;
    public PuzzleManager puzzleManager;

    public VRBlinkLight blinkLight;

    public void TryActivate()
    {
        if (puzzleManager.AreAllCorrect())
        {
            leverLight.SetCorrect(true);
            blinkLight.SetGreen(); // ✅ THIS triggers everything
        }
        else
        {
            leverLight.SetCorrect(false);
        }
    }
}