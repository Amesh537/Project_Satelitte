using UnityEngine;
using TMPro;
using System.Collections;

public class ProximityAudioTrigger : MonoBehaviour
{
    [Header("References")]
    public Transform player;                 // Assign your XR Rig / Player transform
    public AudioSource audioSource;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI tutoralText;

    [Header("Settings")]
    public float triggerRadius = 5f;         // Distance required to trigger
    public float delay = 1f;                 // Subtitle delay
    public bool triggerOnce = true;          // Only play once?

    private bool hasPlayed = false;

    void Start()
    {
        if (subtitleText != null)
            subtitleText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;
        if (triggerOnce && hasPlayed) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= triggerRadius)
        {
            hasPlayed = true;

            if (audioSource != null)
                audioSource.Play();

            StartCoroutine(ShowSubtitle());
        }
    }

    IEnumerator ShowSubtitle()
    {
        yield return new WaitForSeconds(delay);

        if (subtitleText != null)
            subtitleText.gameObject.SetActive(true);

        if (tutoralText != null)
            tutoralText.enabled =false;
    }

    // Optional: Draw radius in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
