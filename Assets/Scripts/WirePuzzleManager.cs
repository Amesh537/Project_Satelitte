using UnityEngine;

public class WirePuzzleManager : MonoBehaviour
{
    public int totalWires = 5;
    private int connectedWires = 0;

    public GameObject miniLight;
    

    private bool hasWon = false;
    [SerializeField] private ObjectiveItem objectiveToComplete;

    public void RegisterCorrectConnection()
    {
        connectedWires++;

        Debug.Log("Connected: " + connectedWires);

        if (!hasWon && connectedWires >= totalWires)
        {
            hasWon = true;
            PlayWinSound();
        }
    }

    void PlayWinSound()
    {
        objectiveToComplete.CompleteObjective();
        Debug.Log("PUZZLE COMPLETE!");
    }
}