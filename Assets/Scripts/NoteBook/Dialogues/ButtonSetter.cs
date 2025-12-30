using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ButtonSetter : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public RectTransform buttonTransform;
    private char[] Letters;

    public void Awake()
    {
        buttonTransform = gameObject.GetComponent<RectTransform>();
    }

    public void SetButtonText(string word)
    {
        buttonText.text = word;
        SetButtonWidth(word);
    }

    private void SetButtonWidth(string word)
    {
        Letters = word.ToCharArray();
        Debug.Log("setting width to " + (40 * Letters.Length) + 40);
        buttonTransform.rect.Set(buttonTransform.rect.x, buttonTransform.rect.y, (40 * Letters.Length) + 40, buttonTransform.rect.height);
        Debug.Log("width after setting " + buttonTransform.rect.width);
    }
}
