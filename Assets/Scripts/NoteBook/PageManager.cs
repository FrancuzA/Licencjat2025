using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    private int numberOfWords = 0;
    public List<GameObject> sides = new();
    private GameObject currentSide;

    private void Start()
    {
        Dependencies.Instance.UnregisterDependency<PageManager>();
        Dependencies.Instance.RegisterDependency<PageManager>(this);

        foreach(Transform child in gameObject.transform)
        {
            sides.Add(child.gameObject);
        }
        currentSide = sides[0];
    }
    public void AddNewWord(GameObject word)
    {
        Instantiate(word,currentSide.transform);
        numberOfWords++;
    }
    public void Update()
    {
        if(numberOfWords >= 6)
        {
            currentSide = sides[1];
        }

        if(numberOfWords >= 12)
        {
            NoteBookManager noteManager = Dependencies.Instance.GetDependancy<NoteBookManager>();
            noteManager.AddPage();
        }
    }
}
