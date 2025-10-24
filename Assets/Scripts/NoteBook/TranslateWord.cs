using TMPro;
using UnityEngine;

public class TranslateWord : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI originalText;
    public Translation _translation;
    void Start()
    {
        inputField.onSubmit.AddListener(SendToManager);
    }

    public void SendToManager(string word)
    {
        if (_translation == null)
        {
            _translation = new Translation(word, $"correct {word}");
        }
        else
        {
            _translation.translatedText = word;
            _translation.correctTranslation = $"correct {word}";
        }

        DictionaryManager.Instance.AddOrUpdate(originalText.text, _translation);
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
