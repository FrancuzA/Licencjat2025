using DialogueSystem.Runtime.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueRunner : MonoBehaviour
{
    public GameObject dialogueScreen;
    [SerializeField] private DialogueRuntimeGraph graph;

    [SerializeField] private ButtonSpawner buttonSpawner;
    [SerializeField] private TextMeshProUGUI actorNameLabel;
    [SerializeField] private TextMeshProUGUI messageLabel;
    [SerializeField] private Button continueButton;

    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private RectTransform choiceButtonsParent;

    private DialogueRuntimeNodes _currentNode;

    private void Start()
    {
        continueButton.onClick.AddListener(MoveNext);
        Dependencies.Instance.RegisterDependency<DialogueRunner>(this);
    }

    private void MoveNext()
    {
        if (_currentNode.OutputPorts.Count == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Dependencies.Instance.GetDependancy<CameraTilt>().inDialogue = false;
            Time.timeScale = 1f;
            dialogueScreen.SetActive(false);
            return;
        }

        _currentNode = _currentNode.OutputPorts[0];
        UpdateUI(_currentNode);
    }

    private void MoveToOutput(int index)
    {
        if (_currentNode.OutputPorts.Count == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Dependencies.Instance.GetDependancy<CameraTilt>().inDialogue = false;
            Time.timeScale = 1f;
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
                buttonSpawner.ReciveMessage(messageNode.message);
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

    public void ChangeDialogue(DialogueRuntimeGraph DialogueGraph)
    {
        graph = DialogueGraph;
    }

    public void OpenDialogue(DialogueRuntimeGraph DialogueGraph)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        Dependencies.Instance.GetDependancy<CameraTilt>().inDialogue = true;
        ChangeDialogue(DialogueGraph);
        dialogueScreen.SetActive(true);
        _currentNode = graph.StartingNode;
        UpdateUI(graph.StartingNode);
        

    }
}
