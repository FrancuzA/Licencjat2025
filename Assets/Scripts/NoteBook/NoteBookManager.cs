using System.Collections.Generic;
using UnityEngine;

public class NoteBookManager : MonoBehaviour
{
    public GameObject noteBookObject;
    private int currentPageIndex = 0;
    private GameObject currentActivePage;
    public List<GameObject> pages;
    private void Start()
    {
        LoadAllPages();
    }

    void Update()
    {
        ProcessInputs();
    }

    public void ProcessInputs()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
           noteBookObject.SetActive(!noteBookObject.activeSelf);
            LoadAllPages();
            Openpage(currentPageIndex);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (CheckForIndexOverflow(currentPageIndex + 1))
            {
                currentPageIndex++;
                Openpage(currentPageIndex);
            }

            else return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentPageIndex - 1 >= 0)
            {
                currentPageIndex--;
                Openpage(currentPageIndex);
            }
            else return;
        }
    }

    public void LoadAllPages()
    {
        pages.Clear();
        foreach(Transform child in noteBookObject.transform)
        {
            pages.Add(child.gameObject);
        }
    }

    public void Openpage(int pageNumber)
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        pages[pageNumber].SetActive(true);
        currentActivePage = pages[pageNumber];
    }

    private bool CheckForIndexOverflow(int pageNumber)
    {
        if (pageNumber >= pages.Count) return false;
        return true;
    }
}
