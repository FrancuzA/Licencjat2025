using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    private List<string> words;
    public GameChoice buttonPrefab;
    public void ReciveMessage(string message)
    {
        words = message.Split(" ").ToList();
    }
}
