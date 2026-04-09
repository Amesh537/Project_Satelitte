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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip completionSound;

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

        if (audioSource != null && completionSound != null)
        {
            audioSource.PlayOneShot(completionSound);
        }

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