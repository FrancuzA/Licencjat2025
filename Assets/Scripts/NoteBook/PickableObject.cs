using UnityEngine;

public class PickableObject : MonoBehaviour, IInteractable
{
    public ItemInfo _itemInfo;
   public void Interact()
    {
        BackPack Backpack = Dependencies.Instance.GetDependancy<BackPack>();
        Backpack.AddToBackpack(gameObject);
        Destroy(gameObject);
    }
}
