using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject DialogueCanva;
    public GameObject TutorialCanva;
    public GameObject NotebookUI;
    public TextMeshProUGUI TutorialTxt;
    public List<string> TutorialMessages;
    private Dependencies _dep;
    private CameraTilt _cameraT;
    private Rigidbody playerRb;
    private bool _tutorialActive;


    private void Start()
    {
        _tutorialActive = false ;
        _dep = Dependencies.Instance;
        _cameraT = _dep.GetDependancy<CameraTilt>();
        playerRb = _dep.GetDependancy<StartPlayerMovement>().gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (TutorialMessages.Count == 0 || _tutorialActive) return;
            playerRb.linearVelocity = Vector3.zero;
            StartCoroutine(Tutorial_Rutine());
        }
    }

    public IEnumerator Tutorial_Rutine()
    {
        _tutorialActive=true;
        StopTime();
        TutorialCanva.SetActive(true);
        TutorialTxt.text = TutorialMessages[0];
        yield return new WaitUntil(() => DialogueCanva.activeInHierarchy == true);
        StopTime();
        TutorialCanva.SetActive(true);
        TutorialTxt.text = TutorialMessages[1];
        yield return new WaitUntil(() => DialogueCanva.activeInHierarchy == false);
        yield return new WaitForSeconds(1);
        StopTime();
        TutorialCanva.SetActive(true);
        TutorialTxt.text = TutorialMessages[2];
        yield return new WaitUntil(() => NotebookUI.activeInHierarchy == true);
        StopTime();
        TutorialCanva.SetActive(true);
        TutorialTxt.text = TutorialMessages[3];
        yield return new WaitUntil(() => NotebookUI.activeInHierarchy == false);
        gameObject.SetActive(false);
    }

    public void RevertTimeAndMouse()
    {
        if (DialogueCanva.activeInHierarchy == true || NotebookUI.activeInHierarchy == true) return;
        _cameraT.UILock = false;   
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StopTime()
    {
        _cameraT.UILock = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
