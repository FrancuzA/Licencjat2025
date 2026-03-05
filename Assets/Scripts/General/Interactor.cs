using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange;
    private Quaternion lastRotation;
    public GameObject interactText;
    public GameObject playerCamera;
    private bool interactableInRange;
    private IInteractable interactable;


    public void Start()
    {
        lastRotation = playerCamera.transform.rotation;
    }
    private void Update()
    {
        if (transform.rotation != lastRotation)
        {
            RotationChanged();
            lastRotation = transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.E) && interactableInRange)
        {
            interactable.Interact();
        }

    }

    private void RotationChanged()
    {
        if (Physics.Raycast(gameObject.transform.position, playerCamera.transform.forward, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
               
                if(interactText != null) interactText.SetActive(true);
                interactable = interactObj;
                interactableInRange = true;
            }
            else { if (interactText != null) interactText.SetActive(false); interactableInRange = false; }
        }
        else { if (interactText != null) interactText.SetActive(false); interactableInRange = false ; }
    }
}
