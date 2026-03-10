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
    public void Awake()
    {
        buttonTransform = gameObject.GetComponent<RectTransform>();
        gameObject.GetComponent<Button>().onClick.AddListener(AddWordToNotebook);
    }

    public void SetButtonText(string word)
    {
        if (!Char.IsLetter(word, 1)) SetButtonOff();
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
        Dependencies.Instance.GetDependancy<NoteBookManager>().AddWordToList(wordOnButton);
    }
}
