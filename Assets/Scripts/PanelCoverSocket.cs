using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanelCoverSocket : MonoBehaviour
{
    public GameObject switchesParent;

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    void Start()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

        // start covered
        switchesParent.SetActive(false);

        socket.selectEntered.AddListener(OnAttach);
        socket.selectExited.AddListener(OnDetach);
    }

    void OnAttach(SelectEnterEventArgs args)
    {
        // cover attached → disable switches
        switchesParent.SetActive(false);
    }

    void OnDetach(SelectExitEventArgs args)
    {
        // cover removed → enable switches
        switchesParent.SetActive(true);
    }
}