using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    private List<string> words;
    public GameObject buttonPrefab;
    public GameObject textRowPref;
    public GameObject currentTextRow;
    public int maxWordCount = 9;
    private int wordCount;
    public void ReciveMessage(string message)
    {
        words = message.Split(" ").ToList();
        SpawnButtons();
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
}
