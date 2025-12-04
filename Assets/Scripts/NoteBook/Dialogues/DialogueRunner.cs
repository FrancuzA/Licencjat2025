using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueRunner : MonoBehaviour
{
    public DialogueRuntimeGraph DialogueGRaph;

    public Image actorImage;
    public TextMeshProUGUI actorname;
    public TextMeshProUGUI message;
    public Button nextButton;

    private void Start()
    {
       // UpdateUI(DialogueGRaph.nodes)
    }

    private void UpdateUI(DialogueRuntimeNodes node)
    {
        switch (node)
        {
            case MessageRuntimeNode messageNode:
                actorImage.sprite = messageNode.avatar;
                actorname.text = messageNode._actor._name;
                message.text = messageNode.message;
                break;
        }
    }
}
