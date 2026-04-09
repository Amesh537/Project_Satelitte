using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class RadialSelection : MonoBehaviour
{
    public InputActionProperty spawnButton;

    [Range(2,10)]
    public int numberOfRadialPart;
    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;
    public float angleBetweenPart = 10;
    public Transform handTransform;
    public Transform headTransform; // XR Camera
    public float spawnDistance = 1.5f; // ~a few feet

    public UnityEvent<int> OnPartSelected;

    public Sprite[] icons;
    public string[] labels;

    public float manualAngleOffset = 0f;   // rotates icon+label around the menu
    public float iconRadius = 0.2f;        // distance from center (can match wedge radius)



    private List<GameObject> spawnedParts = new List<GameObject>();
    private int currentSelectedRadialPart = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnButton.action.WasPressedThisFrame())
        {
            SpawnRadialPart();
        }

        if (spawnButton.action.IsPressed())
        {
            GetSelectedRadialPart();
        }

        if (spawnButton.action.WasReleasedThisFrame())
        {
            HideandTriggerSelected();
        }
    }

    public void HideandTriggerSelected()
    {
        if (currentSelectedRadialPart >= 0)
        {
            OnPartSelected.Invoke(currentSelectedRadialPart);
        }

        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart()
    {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(-radialPartCanvas.up, centerToHandProjected, radialPartCanvas.forward);

        if(angle < 0)
        {
            angle += 360;
        }

        
        currentSelectedRadialPart = (int) angle * numberOfRadialPart / 360;

        for (int i = 0; i < spawnedParts.Count; i++)
        {
            if (i == currentSelectedRadialPart)
            {
                spawnedParts[i].GetComponent<Image>().color = Color.red;
                spawnedParts[i].transform.localScale = 1.1f * Vector3.one;
            }
            else
            {
                spawnedParts[i].GetComponent<Image>().color = Color.white;
                spawnedParts[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void SpawnRadialPart()
    {
        radialPartCanvas.gameObject.SetActive(true);

        radialPartCanvas.position = headTransform.position + headTransform.forward * spawnDistance;
        radialPartCanvas.forward = headTransform.forward;

        foreach (var item in spawnedParts)
            Destroy(item);
        spawnedParts.Clear();

        for (int i = 0; i < numberOfRadialPart; i++)
        {
            float baseAngle = i * 360f / numberOfRadialPart;

            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);

            // ⭐ WEDGE POSITION (unchanged)
            float wedgeRadius = 0.2f;
            spawnedRadialPart.transform.localPosition =
                Quaternion.Euler(0, 0, baseAngle) * Vector3.up * wedgeRadius;

            // ⭐ WEDGE ROTATION (unchanged)
            spawnedRadialPart.transform.localEulerAngles = new Vector3(0, 0, baseAngle);

            // -------------------------------
            // ⭐ ICON + LABEL ORBIT AROUND MENU
            // -------------------------------
            float iconAngle = baseAngle + manualAngleOffset;

            Transform icon = spawnedRadialPart.transform.Find("Icon");
            Transform label = spawnedRadialPart.transform.Find("Label");

            // ⭐ Orbit icons around the menu center (NOT the wedge)
            icon.position =
                radialPartCanvas.position +
                (Quaternion.Euler(0, 0, iconAngle) * Vector3.up * iconRadius);

            label.position =
                radialPartCanvas.position +
                (Quaternion.Euler(0, 0, iconAngle) * Vector3.up * iconRadius);

            // ⭐ Keep icons + labels perfectly vertical in world space
            icon.rotation = Quaternion.identity;
            label.rotation = Quaternion.identity;

            // Fill amount stays the same
            spawnedRadialPart.GetComponent<Image>().fillAmount =
                (1f / numberOfRadialPart) - (angleBetweenPart / 360f);

            // Assign icon + label
            Image iconImage = icon.GetComponent<Image>();
            TextMeshProUGUI labelText = label.GetComponent<TextMeshProUGUI>();

            if (icons != null && i < icons.Length)
                iconImage.sprite = icons[i];

            if (labels != null && i < labels.Length)
                labelText.text = labels[i];

            spawnedParts.Add(spawnedRadialPart);
        }
    }



    void OnEnable()
    {
        spawnButton.action.Enable();
    }

    void OnDisable()
    {
        spawnButton.action.Disable();
    }
}
