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
    private DialogueRunner _dialogueRunner;

    private void Start()
    {
        NPCtransform = transform;
        _dialogueRunner = Dependencies.Instance.GetDependancy<DialogueRunner>();
    }

    public void Interact()
    {
        if (_dialogueRunner.dialogueScreen.activeInHierarchy) return;
        onDialogueStart.Invoke();
        _dialogueRunner.OpenDialogue(NPCDialogue, this);
    }

    public void EndInteraction()
    {
    }

    public void LoadData(SaveData saveData)
    {
        if (saveData.NPCPositions.ContainsKey(NPCName))
            NPCtransform.position = saveData.NPCPositions[NPCName];
    }

    public void SaveData(SaveData saveData)
    {
        saveData.NPCPositions[NPCName] = NPCtransform.position;
    }
}