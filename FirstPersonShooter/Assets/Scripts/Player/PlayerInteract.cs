using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public InputActionReference interactButton;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] GameObject interactCircleUi;
    [SerializeField] GameObject interactCircleUiButton;

    // bool isIntract = false;
    void OnEnable()
    {
        interactButton.action.Enable();
    }
    void OnDisable()
    {
        interactButton.action.Disable();
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position,
            playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactCircleUi.SetActive(true);
                interactCircleUiButton.SetActive(true);

                if (interactButton.action.WasPressedThisFrame())
                {
                    interactable.Interact();
                }
            }
            else{interactCircleUi.SetActive(false); interactCircleUiButton.SetActive(false); }
        }
        else
        {
            interactCircleUi.SetActive(false);
            interactCircleUiButton.SetActive(false);
        }
    }
}
