using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float interactRange;
    public GameObject interactText;
    public GameObject playerCamera;
    public GameObject noteBookObject;
    public GameObject settingsObject;
    public GameObject inventoryObject;
    private bool interactableInRange;
    private IInteractable interactable;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableInRange &&
            noteBookObject.activeInHierarchy == false &&
            settingsObject.activeInHierarchy == false &&
            inventoryObject.activeInHierarchy == false)
        {
            MonoBehaviour mb = interactable as MonoBehaviour;
            if (mb != null && mb.gameObject != null)
            {
                interactable.Interact();
                if (interactText != null) interactText.SetActive(false);
            }
            else
            {
                interactableInRange = false;
                interactable = null;
                if (interactText != null) interactText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            if (interactText != null) interactText.SetActive(true);
            interactable = interactObj;
            interactableInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactObj))
        {
            if (interactText != null) interactText.SetActive(false);
            interactable.EndInteraction();
            interactableInRange = false;
        }
    }
}
