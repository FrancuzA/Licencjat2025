using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackPack : MonoBehaviour
{
    public List<ItemInfo> collected;
    public float dropOffset;
    public float dropHeight;

    public void Start()
    {
        Dependencies.Instance.RegisterDependency<BackPack>(this);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            DropFromBackPack(collected[0].ItemPrefab);
        }
    }

    public void AddToBackpack(GameObject item)
    {
        AudioManager Audio = Dependencies.Instance.GetDependancy<AudioManager>();
        Audio.PickDrop = "PickUp";
        Audio.PlayPickDrop();
        ItemInfo IItem = item.GetComponent<PickableObject>()._itemInfo;
        collected.Add(IItem);
    }

    public void DropFromBackPack(GameObject item)
    {
        ItemInfo IItem = item.GetComponent<PickableObject>()._itemInfo;
        collected.Remove(IItem);
        GameObject SpawnObject = IItem.ItemPrefab;

        Vector3 dropPosition = CalculateDropPosition();
        Instantiate(SpawnObject, dropPosition, Quaternion.identity);

        AudioManager Audio = Dependencies.Instance.GetDependancy<AudioManager>();
        Audio.PickDrop = "Drop";
        Audio.PlayPickDrop();
    }

    private Vector3 CalculateDropPosition()
    {
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize();

        Vector3 dropPosition = transform.position + (forwardDirection * dropOffset);
        dropPosition.y += dropHeight;

        return dropPosition;
    }
}
