using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
   public void Interact()
    {
        Debug.Log("Object picked Up ");
        Destroy(gameObject);
    }
}
