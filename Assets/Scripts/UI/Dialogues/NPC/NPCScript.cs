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

    private DialogueRunner GetDialogueRunner()
    {
        DialogueRunner runner = Dependencies.Instance.GetDependancy<DialogueRunner>();
        if (runner == null)
            Debug.LogError($"[NPCScript] {gameObject.name} could not find DialogueRunner in Dependencies.");
        return runner;
    }

    public void Interact()
    {
        DialogueRunner runner = GetDialogueRunner();
        if (runner == null) return;

        if (NPCDialogue == null)
        {
            Debug.LogError($"[NPCScript] {gameObject.name} has no DialogueRuntimeGraph assigned.");
            return;
        }

        if (runner.dialogueScreen.activeInHierarchy) return;

        onDialogueStart.Invoke();
        runner.OpenDialogue(NPCDialogue, this);
    }

    public void EndInteraction() { }

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