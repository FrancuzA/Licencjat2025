using TMPro;
using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable
{
    public GameObject TextPanel; 

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        TextPanel.SetActive(true);
    }
}
