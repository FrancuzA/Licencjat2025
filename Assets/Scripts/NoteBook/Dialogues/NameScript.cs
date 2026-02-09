using System;
using TMPro;
using UnityEngine;

public class NameScript : MonoBehaviour
{
    public string PlayerName;
    public string CurrentName;
    public TMP_SpriteAsset spriteAsset;
    void Awake()
    {
        Dependencies.Instance.RegisterDependency<NameScript>(this);
    }


    private void FixedUpdate()
    {
        CurrentName = GetComponent<TMP_Text>().text;
        if (namesMatch() && GetComponent<TMP_Text>().spriteAsset != null)
        {
            GetComponent<TMP_Text>().spriteAsset = null;
        }
        else if(GetComponent<TMP_Text>().spriteAsset == null) GetComponent<TMP_Text>().spriteAsset = spriteAsset;
    }

    public bool namesMatch()
    {
        Debug.Log($"current name: {CurrentName}, PlayerName: {PlayerName} ");
        if (CurrentName == PlayerName) return true;
        else return false;
    }
}
