using UnityEngine;

public class ObjectiveItem : MonoBehaviour
{
    [Header("Objective")]
    [SerializeField] private string displayName = "New Objective";
    [SerializeField] private HUDObjectiveManager hudManager;
    [SerializeField] private bool registerOnStart = true;

    private bool isComplete = false;

    public string DisplayName => displayName;
    public bool IsComplete => isComplete;

    private void Start()
    {
        if (registerOnStart && hudManager != null)
        {
            hudManager.RegisterObjective(this);
        }
    }

    public void CompleteObjective()
    {
        if (isComplete) return;

        isComplete = true;

        if (hudManager != null)
            hudManager.NotifyObjectiveChanged();
    }

    public void ResetObjective()
    {
        isComplete = false;

        if (hudManager != null)
            hudManager.NotifyObjectiveChanged();
    }

    public void ResetObjectiveSilently()
    {
        isComplete = false;
    }
}