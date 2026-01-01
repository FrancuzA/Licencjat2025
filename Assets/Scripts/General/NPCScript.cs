using TMPro;
using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable
{
    public DialogueRuntimeGraph NPCDialogue;
    public void Interact()
    {
        Dependencies.Instance.GetDependancy<DialogueRunner>().OpenDialogue(NPCDialogue);
    }
}
