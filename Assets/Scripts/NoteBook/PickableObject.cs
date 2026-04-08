using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    public ItemInfo _itemInfo;

    public void EndInteraction()
    {

    }

    public void Interact()
    {
        BackPack Backup = Dependencies.Instance.GetDependancy<BackPack>();
        Backup.AddToBackpack(gameObject);
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject);
    }
}
