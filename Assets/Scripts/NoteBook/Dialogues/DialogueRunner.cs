using DialogueSystem.Runtime.Nodes;
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

    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private RectTransform choiceButtonsParent;

    private DialogueRuntimeNodes _currentNode;

    private void Start()
    {
        _currentNode = graph.StartingNode;
        UpdateUI(graph.StartingNode);
        continueButton.onClick.AddListener(MoveNext);
    }

    private void MoveNext()
    {
        if (_currentNode.OutputPorts.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        _currentNode = _currentNode.OutputPorts[0];
        UpdateUI(_currentNode);
    }

    private void MoveToOutput(int index)
    {
        if (_currentNode.OutputPorts.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        _currentNode = _currentNode.OutputPorts[index];
        UpdateUI(_currentNode);
    }

    private void UpdateUI(DialogueRuntimeNodes node)
    {
        switch (node)
        {
            case MessageRuntimeNode messageNode:
                actorImage.sprite = messageNode._actor._avatar;
                messageLabel.text = messageNode.message;
                actorNameLabel.text = messageNode._actor._name;

                choiceButtonsParent.gameObject.SetActive(false);
                continueButton.gameObject.SetActive(true);
                break;
            case ChoiceRuntimeNode choiceNode:
                choiceButtonsParent.DestroyAllChildren();

                choiceButtonsParent.gameObject.SetActive(true);
                continueButton.gameObject.SetActive(false);

                for (var i = 0; i < choiceNode.OutputPorts.Count; i++)
                {
                    int outputIndex = i;
                    Button spawnedButton = Instantiate(choiceButtonPrefab, choiceButtonsParent);
                    spawnedButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{choiceNode.ChoiceAsset.Choices[i]}";

                    spawnedButton.onClick.AddListener(() =>
                    {
                        MoveToOutput(outputIndex);
                    });
                }
                break;
        }
    }
}
