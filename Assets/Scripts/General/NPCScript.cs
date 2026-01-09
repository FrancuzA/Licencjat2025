using TMPro;
using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable, ISaveSystemElement
{
    public DialogueRuntimeGraph NPCDialogue;
    public string NPCName = " ";
    private Transform NPCtransform;

    private void Start()
    {
        NPCtransform = transform;
    }
    public void Interact()
    {
        Dependencies.Instance.GetDependancy<DialogueRunner>().OpenDialogue(NPCDialogue);
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
}
