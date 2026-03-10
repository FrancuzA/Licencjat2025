using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    private List<string> words;
    public GameObject buttonPrefab;
    public GameObject textRowPref;
    public GameObject plainTextPref;
    public GameObject currentTextRow;
    public int maxWordCount = 9;
    private int wordCount;
    public void ReciveMessage(string message, string name)
    {
        
        if (name == "NOAH")
        {
            SpawnText(message);
        }
        else
        { 
            words = message.Split(" ").ToList();
            SpawnButtons();
        }
    }

    private void SpawnButtons()
    {
        gameObject.transform.DestroyAllChildren();
        currentTextRow = Instantiate(textRowPref, Vector3.zero, Quaternion.identity, gameObject.transform);
        wordCount = 0;
        foreach (var word in words) 
        {
            if(wordCount == maxWordCount)
            {
                currentTextRow = Instantiate(textRowPref, Vector3.zero, Quaternion.identity, gameObject.transform);
                wordCount = 0; 
            }
            GameObject spawnedButton = Instantiate(buttonPrefab, currentTextRow.transform);
            spawnedButton.GetComponent<ButtonSetter>().SetButtonText(word);
            wordCount++;
        }
    }

    public void SpawnText(string text)
    {
        gameObject.transform.DestroyAllChildren();
        GameObject _text = Instantiate(plainTextPref, Vector3.zero, Quaternion.identity, gameObject.transform);
        _text.GetComponent<TextMeshProUGUI>().text = text;
    }
}
