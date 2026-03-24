using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HUDObjectiveManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI objectivesText;
    [SerializeField] private string title = "Objectives";
    [SerializeField] private bool showCount = true;

    [Header("Colors")]
    [SerializeField] private Color incompleteColor = Color.red;
    [SerializeField] private Color completeColor = Color.green;

    [Header("Audio")]
    [SerializeField] private AudioClip allCompleteClip;
    [SerializeField] private AudioSource audioSource;

    private readonly List<ObjectiveItem> objectiveOrder = new List<ObjectiveItem>();
    private bool allCompleteTriggered = false;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void RegisterObjective(ObjectiveItem objective)
    {
        if (objective == null) return;
        if (objectiveOrder.Contains(objective)) return;

        objectiveOrder.Add(objective);
        RefreshUI();
    }

    public void NotifyObjectiveChanged()
    {
        RefreshUI();
        CheckAllComplete();
    }

    public void RefreshUI()
    {
        if (objectivesText == null) return;

        int total = objectiveOrder.Count;
        int completed = 0;

        for (int i = 0; i < objectiveOrder.Count; i++)
        {
            if (objectiveOrder[i] != null && objectiveOrder[i].IsComplete)
                completed++;
        }

        string header = showCount ? $"{title} ({completed}/{total})" : title;
        string result = header + "\n";

        for (int i = 0; i < objectiveOrder.Count; i++)
        {
            ObjectiveItem obj = objectiveOrder[i];
            if (obj == null) continue;

            Color c = obj.IsComplete ? completeColor : incompleteColor;
            string hex = ColorUtility.ToHtmlStringRGB(c);

            result += $"<color=#{hex}>• {obj.DisplayName}</color>\n";
        }

        objectivesText.text = result;
    }

    private void CheckAllComplete()
    {
        if (allCompleteTriggered) return;
        if (objectiveOrder.Count == 0) return;

        for (int i = 0; i < objectiveOrder.Count; i++)
        {
            ObjectiveItem obj = objectiveOrder[i];
            if (obj == null || !obj.IsComplete)
                return;
        }

        allCompleteTriggered = true;

        if (allCompleteClip != null)
        {
            Vector3 pos = audioSource != null ? audioSource.transform.position : transform.position;
            AudioSource.PlayClipAtPoint(allCompleteClip, pos);
        }
    }

    public void ResetAllObjectives()
    {
        allCompleteTriggered = false;

        for (int i = 0; i < objectiveOrder.Count; i++)
        {
            if (objectiveOrder[i] != null)
                objectiveOrder[i].ResetObjectiveSilently();
        }

        RefreshUI();
    }
}