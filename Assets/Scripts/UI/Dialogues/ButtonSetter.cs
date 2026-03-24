using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetter : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public RectTransform buttonTransform;
    private char[] Letters;
    private string wordOnButton;
    private int buttonWidth = 0;

    private DictionaryManager _dictionary;
    private PopUpManager _popUpmanager;
    public void Awake()
    {
        buttonTransform = gameObject.GetComponent<RectTransform>();
        gameObject.GetComponent<Button>().onClick.AddListener(AddWordToNotebook);
        _dictionary = DictionaryManager.Instance;
        _popUpmanager = Dependencies.Instance.GetDependancy<PopUpManager>();
    }

    public void SetButtonText(string word)
    {
        if(!string.IsNullOrEmpty(word)) if (!Char.IsLetter(word, 0)) SetButtonOff();
        buttonText.text = word;
        wordOnButton = word;
        SetButtonWidth(word);
    }

    private void SetButtonOff()
    {
        Color buttonColor = gameObject.GetComponent<Image>().color;
        buttonColor.a = 0;
        gameObject.GetComponent<Image>().color = buttonColor;
        gameObject.GetComponent<Button>().interactable = false;
    }

    private void SetButtonWidth(string word)
    {
        Letters = word.ToCharArray();
        buttonWidth = (40 * Letters.Length) + 40;
        buttonTransform.sizeDelta = new Vector2(buttonWidth, buttonTransform.sizeDelta.y);
    }

    private void AddWordToNotebook()
    {

        if (_dictionary.Contains(wordOnButton))
        {
            Debug.Log("word already in dictionary");
            return;
        }
        _popUpmanager.StartPopUp($"NEW WORD ADDED TO JURNAL - {wordOnButton}");
        Dependencies.Instance.GetDependancy<NoteBookManager>().AddWordToList(wordOnButton);
    }
}
