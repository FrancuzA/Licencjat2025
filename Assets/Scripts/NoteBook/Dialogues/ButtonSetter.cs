using TMPro;
using UnityEngine;

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
    }

    public void SetButtonText(string word)
    {
        buttonText.text = word;
        wordOnButton = word;
        SetButtonWidth(word);
    }

    private void SetButtonWidth(string word)
    {
        Letters = word.ToCharArray();
        buttonWidth = (40 * Letters.Length) + 40;
        buttonTransform.sizeDelta = new Vector2(buttonWidth, buttonTransform.sizeDelta.y);
    }
}
