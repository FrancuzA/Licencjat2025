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
        if (Input.GetKeyDown(KeyCode.E) && interactableInRange && noteBookObject.activeInHierarchy == false && settingsObject.activeInHierarchy == false && inventoryObject.activeInHierarchy == false)
        {
            interactable.Interact();
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
            interactableInRange = false;
        }
    }
}
