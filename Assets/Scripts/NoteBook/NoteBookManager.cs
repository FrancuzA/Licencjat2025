using Commands;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteBookManager : MonoBehaviour
{
    [Header("General")]
    public GameObject noteBookObject;
    public GameObject settingsObject;
    public GameObject inventoryObject;
    private GameObject currentActivePage;
    public GameObject pagePrefab;
    public GameObject wordPref;
    public List<GameObject> pages;
    public List<string> wordsToAdd = new List<string>();
    private int currentPageIndex = 0;
    public bool isWriting = false;

    [Header("Audio")]
    public EventReference NotebookSoundRef;
    public EventReference PageTurnSoundRef;
    private EventInstance NotebookSoundInstance;
    private EventInstance PageTurnSoundInstance;

    
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        LoadAllPages();
        CommandsManager.Instance.RegisterInstance(this);
        Dependencies.Instance.RegisterDependency<NoteBookManager>(this);
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

    public void SendWordToAdd(string originalWord)
    {
        PageManager currentPage = Dependencies.Instance.GetDependancy<PageManager>();
        currentPage.AddNewWord(wordPref, originalWord);
    }
    public void OpenCloseNotebook()
    {
        NotebookSoundInstance = RuntimeManager.CreateInstance(NotebookSoundRef);
        if (noteBookObject.activeInHierarchy || settingsObject.activeInHierarchy || inventoryObject.activeInHierarchy)

        {
            NotebookSoundInstance.setParameterByName("NoteBookState", 1);
            NotebookSoundInstance.start();
            NotebookSoundInstance.release();
            Time.timeScale = 1f;
            Dependencies.Instance.GetDependancy<CameraTilt>().inMenu = false;
            noteBookObject.SetActive(false);
            settingsObject.SetActive(false);
            inventoryObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        if (!noteBookObject.activeInHierarchy && !settingsObject.activeInHierarchy && !inventoryObject.activeInHierarchy)
        {
            NotebookSoundInstance.setParameterByName("NoteBookState", 0);
            NotebookSoundInstance.start();
            NotebookSoundInstance.release();
            Time.timeScale = 0f;
            Dependencies.Instance.GetDependancy<CameraTilt>().inMenu = true;
            noteBookObject.SetActive(true);
            LoadAllPages();
            Openpage(currentPageIndex);
            StartCoroutine(CheckForNewWords());
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }
    }
   
    public IEnumerator CheckForNewWords()
    {
        yield return new WaitUntil(() => Dependencies.Instance.GetDependancy<PageManager>() != null);
        if (wordsToAdd.Count > 0)
        {
            foreach (var word in wordsToAdd)
            {
               SendWordToAdd(word);
            }
            wordsToAdd.Clear();
        }
    }

    public void AddWordToList(string word)
    {
        wordsToAdd.Add(word);
    }

    public void ExitToMenu()
    {
        Debug.Log("Exiting to main menu");
        Time.timeScale = 1f;
        Dependencies.Instance.GetDependancy<CameraTilt>().inMenu = false;
        noteBookObject.SetActive(false);
        settingsObject.SetActive(false);
        inventoryObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadSceneAsync(0);
    }
}
