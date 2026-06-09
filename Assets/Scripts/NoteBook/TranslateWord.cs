using System.Collections;
using TMPro;
using UnityEngine;

public class TranslateWord : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI originalText;
    private DictionaryManager manager;
    private CutSceneManager cutSceneManager;
    private Animator _animator;
    private PopUpManager _popupmanager;
    private int wrongCountH;
    private int wrongCountP;
    private int wrongCountS;
    void Start()
    {
        wrongCountH = 0;
        wrongCountP = 0;
        wrongCountS = 0;
        inputField.onSubmit.AddListener(SendToManager);
        inputField.onSubmit.AddListener(CheckIFCorrect);
        manager = DictionaryManager.Instance;
        _animator = GetComponentInParent<Animator>();
        cutSceneManager = Dependencies.Instance.GetDependancy<CutSceneManager>();
        _popupmanager = Dependencies.Instance.GetDependancy<PopUpManager>();
    }

    public void SendToManager(string word)
    {
        manager.AddOrUpdate(originalText.text, word);
    }

    private void CheckIFCorrect(string word)
    {
        if (manager.CheckTranslation(originalText.text, word)) StartCoroutine(GoodTranslation());
        else StartCoroutine(BadTranslation());
    }
    public void BlockNotebookInteraction()
    {
        NoteBookManager noteBookManager = Dependencies.Instance.GetDependancy<NoteBookManager>();
        noteBookManager.isWriting = true;
    }

    public void UnlockNotebookInteraction()
    {
        NoteBookManager noteBookManager = Dependencies.Instance.GetDependancy<NoteBookManager>();
        noteBookManager.isWriting = false;
    }

    private IEnumerator GoodTranslation()
    {
        Dependencies.Instance.GetDependancy<WordTranslationHub>().NewWordTranslated(originalText.text);
        cutSceneManager.AddCorrectWord();
        inputField.interactable = false;
        _animator.SetTrigger("Good");
        yield return null;
    }

    private IEnumerator BadTranslation()
    {
        if(originalText.text == "c")
        {
            
        }

        switch (originalText.text)
        {
            case "c":
                wrongCountH++;
                if (wrongCountH == 3) _popupmanager.StartHintPopUp("<i>*MAYBE HE IS GREATING ME?*");
                break;
            case "p":
                wrongCountP++;
                if (wrongCountP == 3) _popupmanager.StartHintPopUp("<i>*MAYBE HE IS POINTING TO AN OBJECT TO HIS RIGHT?*");
                break;
            case "s":
                wrongCountS++;
                if (wrongCountS == 3) _popupmanager.StartHintPopUp("<i>*MAYBE HE IS POINTING AT SOMETHING FAR ON THE HORIZON?*");
                break;
        }

        _animator.SetTrigger("Bad");
        yield return null;
    }

}
