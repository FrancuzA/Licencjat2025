using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueRunner : MonoBehaviour
{
    [SerializeField] private DialogueRuntimeGraph graph;

    [Space, SerializeField] private Image actorImage;
    [SerializeField] private TextMeshProUGUI actorNameLabel;
    [SerializeField] private TextMeshProUGUI messageLabel;
    [SerializeField] private Button continueButton;

    private int _currentNodeIndex = 0;

    private void Start()
    {
        UpdateUI(graph.Nodes[_currentNodeIndex]);
        continueButton.onClick.AddListener(MoveNext);
    }

    private void MoveNext()
    {
        _currentNodeIndex++;

        if (_currentNodeIndex >= graph.Nodes.Count)
        {
            gameObject.SetActive(false);
            return;
        }

        UpdateUI(graph.Nodes[_currentNodeIndex]);
    }

    private void UpdateUI(DialogueRuntimeNode node)
    {
        switch (node)
        {
            case MessageRuntimeNode messageNode:
                actorImage.sprite = messageNode.Actor.Sprite;
                messageLabel.text = messageNode.Message;
                actorNameLabel.text = messageNode.Actor.Name;
                break;
        }
    }
}
