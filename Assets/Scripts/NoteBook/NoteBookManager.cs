using Commands;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookManager : MonoBehaviour
{
    [Header("General")]
    public GameObject noteBookObject;
    public GameObject settingsObject;
    private GameObject currentActivePage;
    public GameObject pagePrefab;
    public List<GameObject> pages;
    private int currentPageIndex = 0;
    public bool isWriting = false;

    [Header("Audio")]
    public EventReference NotebookSoundRef;
    public EventReference PageTurnSoundRef;
    private EventInstance NotebookSoundInstance;
    private EventInstance PageTurnSoundInstance;

    
    private void Start()
    {
        LoadAllPages();
        StartCoroutine(TryToRegister());
        CommandsManager.Instance.RegisterInstance(this);
    }

    void Update()
    {
        if(!isWriting)
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
            if (child.CompareTag("Page"))
                pages.Add(child.gameObject);
        }
    }

    [Command("OpenPage", "opens the page x if it exist")]
    public void Openpage(int pageNumber)
    {
        PageTurnSoundInstance = RuntimeManager.CreateInstance(PageTurnSoundRef);
        PageTurnSoundInstance.start();
        PageTurnSoundInstance.release();
        if (pageNumber >= pages.Count) return;
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
    public void AddPage()
    {
        GameObject lastPage = Dependencies.Instance.GetDependancy<PageManager>().gameObject;
        lastPage.GetComponent<PageManager>().enabled = false;
        GameObject newPage = Instantiate(pagePrefab,noteBookObject.transform.position,Quaternion.identity,noteBookObject.transform);
        newPage.AddComponent<PageManager>();
        LoadAllPages();
    }

    public void SendWordToAdd(GameObject word)
    {
        PageManager currentPage = Dependencies.Instance.GetDependancy<PageManager>();
        currentPage.AddNewWord(word);
    }
    public void OpenCloseNotebook()
    {
        NotebookSoundInstance = RuntimeManager.CreateInstance(NotebookSoundRef);
        if (noteBookObject.activeInHierarchy || settingsObject.activeInHierarchy)
        {
            NotebookSoundInstance.setParameterByName("NoteBookState", 1);
            NotebookSoundInstance.start();
            NotebookSoundInstance.release();
            Time.timeScale = 1f;
            noteBookObject.SetActive(false);
            settingsObject.SetActive(false);
            return;
        }

        if (!noteBookObject.activeInHierarchy && !settingsObject.activeInHierarchy)
        {
            NotebookSoundInstance.setParameterByName("NoteBookState", 0);
            NotebookSoundInstance.start();
            NotebookSoundInstance.release();
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
