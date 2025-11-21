using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "MyGame/Item")]
public class ItemInfo : ScriptableObject
{
    public string ItemName;
    public Sprite ItemUI;
    public GameObject ItemPrefab;
}

