using UnityEngine;

public class MonologTrigger : MonoBehaviour
{
    public DialogueRuntimeGraph Monolog;

    private void OnTriggerEnter(Collider other)
    {
        Dependencies.Instance.GetDependancy<DialogueRunner>().OpenDialogue(Monolog, null);
    }
}
