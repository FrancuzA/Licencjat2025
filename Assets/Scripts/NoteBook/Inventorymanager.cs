using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventorymanager : MonoBehaviour
{
    public List<Transform> Cells;
    public List<Transform> HotKeys;
    public List<ItemInfo> AllItems;
    public List<ItemInfo> HotKeyItems;
    public GameObject itemPref;

    

    public void OnEnable()
    {
       
       PopulateInventory();
    }

    private void GetItems()
    {
        AllItems = Dependencies.Instance.GetDependancy<BackPack>().collected;
    }

    public void PopulateInventory()
    {
        GetItems();
        ClearInventory();
        int i = 0;
        foreach(ItemInfo item in AllItems)
        {
            GameObject UIItem = CreateUIItem(item);
            UIItem.transform.SetParent(Cells[i].transform);
            UIItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            i++;
        }
    }

    public void ClearInventory() 
    {
        foreach (Transform cell in Cells)
        {
            foreach (Transform child in cell)
            {
                Debug.Log("clearing...");
                Destroy(child.gameObject);
            }
        }
    }

    public GameObject CreateUIItem(ItemInfo IInfo)
    {
        GameObject UIItem = Instantiate(itemPref);
        Image img = UIItem.GetComponent<Image>();
        img.sprite = IInfo.ItemUI;
        DragUIElement UIItemScr = UIItem.GetComponent<DragUIElement>();
        UIItemScr.itemInfo = IInfo;
        return UIItem;
    }
}
