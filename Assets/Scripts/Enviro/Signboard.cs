using System.Dynamic;
using UnityEngine;
using TMPro;

public class Signboard : MonoBehaviour, IInteractable
{
    private SettingsManager settings;
    private DictionaryManager _dictionary;
    private PopUpManager _popUpmanager;
    private NoteBookManager _notebook;
    private float currentFOV = 0;
    private bool hasInteracted=false;

    public void Start()
    {
        settings = Dependencies.Instance?.GetDependancy<SettingsManager>();
        _dictionary = DictionaryManager.Instance;
        _popUpmanager = Dependencies.Instance?.GetDependancy<PopUpManager>();
        _notebook = Dependencies.Instance?.GetDependancy<NoteBookManager>();
    }
    public void EndInteraction()
    {
        if (!hasInteracted) return;
        settings?.SetFOV(currentFOV);
        hasInteracted = false;

    }

    public void Interact()
    {
        if (hasInteracted) return;
        currentFOV = PlayerPrefs.GetFloat("FOV");
        settings?.HardSetFOV(20);
        hasInteracted = true;
        AddWordToNotebook();
    }

    private void AddWordToNotebook()
    {
        string wordOnSign = GetComponent<TMPro.TextMeshProUGUI>().text;

        if (_dictionary.Contains(wordOnSign))
        {
            return;
        }
        _popUpmanager?.StartPopUp($"NEW WORD ADDED TO JURNAL - {wordOnSign}");
       _notebook?.AddWordToList(wordOnSign);
        _notebook?.StartCoroutine(_notebook?.CheckForNewWords());

    }
}
