using UnityEngine;

public class MonologTrigger : MonoBehaviour
{
    public DialogueRuntimeGraph Monolog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Dependencies.Instance.GetDependancy<DialogueRunner>().OpenDialogue(Monolog, null);
            gameObject.SetActive(false);
        }
        
    }
}
