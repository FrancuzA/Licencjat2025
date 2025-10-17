using Commands;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookManager : MonoBehaviour
{
    public GameObject noteBookObject;
    private GameObject currentActivePage;
    public GameObject pagePrefab;
    public List<GameObject> pages;
    private int currentPageIndex = 0;
    private void Start()
    {
        LoadAllPages();
        StartCoroutine(TryToRegister());
        CommandsManager.Instance.RegisterInstance(this);
    }

    void Update()
    {
        ProcessInputs();
    }

    public void ProcessInputs()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
          OpenCloseNotebook();
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

    [Command("AddPage", "Adds an Page")]
    public void AddPage(GameObject lastPage)
    {
        lastPage.GetComponent<PageManager>().enabled = false;
        GameObject newPage = Instantiate(pagePrefab,noteBookObject.transform.position,Quaternion.identity,noteBookObject.transform);
        newPage.AddComponent<PageManager>();
    }

    public void SendWordToAdd(GameObject word)
    {
        PageManager currentPage = Dependencies.Instance.GetDependancy<PageManager>();
        currentPage.AddNewWord(word);
    }
    public void OpenCloseNotebook()
    {
        if (noteBookObject.activeInHierarchy)
        {
            Time.timeScale = 1f;
            noteBookObject.SetActive(false);
            return;
        }

        if (!noteBookObject.activeInHierarchy)
        {
            Time.timeScale = 0f;
            noteBookObject.SetActive(true);
            LoadAllPages();
            Openpage(currentPageIndex);
            return;
        }
    }
    IEnumerator TryToRegister()
    {
        yield return new WaitUntil(() => Dependencies.Instance != null);
        Dependencies.Instance.RegisterDependency<NoteBookManager>(this);
    }
    
}
