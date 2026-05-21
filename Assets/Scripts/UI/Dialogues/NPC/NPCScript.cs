using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NPCScript : MonoBehaviour, IInteractable, ISaveSystemElement
{
    public DialogueRuntimeGraph NPCDialogue;
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueStop;
    public string NPCName = " ";
    private Transform NPCtransform;

    private void Start()
    {
        NPCtransform = transform;
    }
    public void Interact()
    {
        onDialogueStart.Invoke();
        Dependencies.Instance.GetDependancy<DialogueRunner>().OpenDialogue(NPCDialogue, this);
    }

    public void LoadData(SaveData saveData)
    {
        if (saveData.NPCPositions.ContainsKey(NPCName))
        {
            NPCtransform.position = saveData.NPCPositions[NPCName];
        }
           
    }

    public void SaveData(SaveData saveData)
    {
        saveData.NPCPositions[NPCName] = NPCtransform.position;
    }

    public void EndInteraction()
    {
        
    }
}
