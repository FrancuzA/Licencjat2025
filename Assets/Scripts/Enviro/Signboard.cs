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
    public void Interact()
    {
        if (hasInteracted) return;
        currentFOV = settings.FOVSlider.value; 
        settings.HardSetFOV(20f);
        hasInteracted = true;
        AddWordToNotebook();
    }

    public void EndInteraction()
    {
        if (!hasInteracted) return;
        settings.HardSetFOV(Mathf.Lerp(60f, 110f, currentFOV)); 
        hasInteracted = false;
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
