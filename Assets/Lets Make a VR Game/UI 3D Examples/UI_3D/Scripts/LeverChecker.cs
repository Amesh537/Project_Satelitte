using UnityEngine;

public class LeverPuzzleCheck : MonoBehaviour
{
    public SliderLight leverLight;
    public PuzzleManager puzzleManager;

    public void TryActivate()
    {
        if (puzzleManager.AreAllCorrect())
        {
            leverLight.SetCorrect(true); // success
        }
        else
        {
            leverLight.SetCorrect(false);

            // 🔥 RESET EVERYTHING
            puzzleManager.ResetAllLights();
        }
    }
}