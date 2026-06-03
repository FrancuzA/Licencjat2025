using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject DialogueCanva;
    public GameObject NotebookUI;
    public GameObject clickToAdd;
    public Transform npc;
    public List<DialogueRuntimeGraph> monologPhases = new List<DialogueRuntimeGraph>();

    private Dependencies _dep;
    private DialogueRunner _dialogueRunner;
    private Rigidbody _playerRb;
    public Transform playerTransform;
    private bool _tutorialActive;
    private bool _stopLooking;
    private bool _isShouting;

    private void Start()
    {
        _tutorialActive = false;
        _isShouting = false;
        _dep = Dependencies.Instance;
        _playerRb = _dep.GetDependancy<StartPlayerMovement>().gameObject.GetComponent<Rigidbody>();
        playerTransform = _playerRb.gameObject.transform;
        _dialogueRunner = _dep.GetDependancy<DialogueRunner>();
    }

    private void FixedUpdate()
    {
        if (_stopLooking) return;

        Vector3 flatDirection = new Vector3(
            npc.position.x - playerTransform.position.x,
            0f,
            npc.position.z - playerTransform.position.z
        );

        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
        _playerRb.MoveRotation(targetRotation);
        Debug.Log(playerTransform.rotation.eulerAngles);
    }

    public void startMonologue()
    {
        if (monologPhases.Count == 0 || _tutorialActive) return;
        _playerRb.linearVelocity = Vector3.zero;
        StartCoroutine(Tutorial_Rutine());
    }

    public void StartShouting()
    {
        _isShouting=true;
    }

    public IEnumerator Tutorial_Rutine()
    {
        _tutorialActive = true;
        _dialogueRunner.OpenDialogue(monologPhases[0], null);
        yield return new WaitUntil(() => !DialogueCanva.activeInHierarchy);

        _stopLooking = true;
       

        yield return new WaitUntil(() => _isShouting);
        _stopLooking=false;
        _dialogueRunner.OpenDialogue(monologPhases[1], null);
        yield return new WaitUntil(() => !DialogueCanva.activeInHierarchy);
        _stopLooking=true;
        clickToAdd.SetActive(true);
        yield return new WaitUntil(() => DialogueCanva.activeInHierarchy);
        yield return new WaitUntil(() => !DialogueCanva.activeInHierarchy);
        yield return new WaitForSeconds(1);

        clickToAdd.SetActive(false);
        _dialogueRunner.OpenDialogue(monologPhases[2], null);
        yield return new WaitUntil(() => NotebookUI.activeInHierarchy);

        gameObject.SetActive(false);
    }
}