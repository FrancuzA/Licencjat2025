using TMPro;
using UnityEngine;

public class TranslateWord : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI originalText;
    void Start()
    {
        inputField.onSubmit.AddListener(SendToManager);
    }

    public void SendToManager(string word)
    {
        DictionaryManager.Instance.AddOrUpdate(originalText.text, word);
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

}
