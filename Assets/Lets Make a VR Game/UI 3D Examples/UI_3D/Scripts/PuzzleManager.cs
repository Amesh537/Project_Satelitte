using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public SliderLight[] allLights;
    [SerializeField] private ObjectiveItem objectiveToComplete;

    public bool AreAllCorrect()
    {
        foreach (var light in allLights)
        {
            if (!light.isCorrect)
                return false;
        }
        objectiveToComplete.CompleteObjective();
        return true;
    }

    // ✅ ADD THIS
    public void ResetAllLights()
    {
        foreach (var light in allLights)
        {
            light.SetCorrect(false);
        }

        Debug.Log("Puzzle Reset!");
    }
}