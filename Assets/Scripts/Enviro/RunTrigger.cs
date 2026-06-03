using UnityEngine;
using UnityEngine.Events;

public class RunTrigger : MonoBehaviour
{
    public UnityEvent StartRunning;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) StartRunning.Invoke();

    }
}
