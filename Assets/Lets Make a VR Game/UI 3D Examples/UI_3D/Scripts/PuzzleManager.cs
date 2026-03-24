using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public SliderLight[] lights;
    [SerializeField] private ObjectiveItem objectiveToComplete;
    
    public bool AreAllCorrect()
    {
        foreach (var light in lights)
        {
            if (!light.isCorrect)
                return false;
        }
        objectiveToComplete.CompleteObjective();
        return true;
    }
}