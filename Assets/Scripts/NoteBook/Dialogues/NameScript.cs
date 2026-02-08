using System;
using TMPro;
using UnityEngine;

public class NameScript : MonoBehaviour
{
    public string PlayerName;
    public string CurrentName;
    void Start()
    {
        Dependencies.Instance.RegisterDependency<NameScript>(this);
    }

    private void FixedUpdate()
    {
        CurrentName = GetComponent<TMP_Text>().text;
    }

    public bool namesMatch()
    {
        if (CurrentName == PlayerName) return true;
        else return false;
    }
}
