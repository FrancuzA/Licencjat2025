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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (TutorialMessages.Count == 0) return;
            StartCoroutine(Tutorial_Rutine());
        }
    }

    public IEnumerator Tutorial_Rutine()
    {
        StopTime();
        TutorialCanva.SetActive(true);
        TutorialTxt.text = TutorialMessages[0];
        yield return new WaitUntil(() => DialogueCanva.activeInHierarchy == true);
        StopTime();
        TutorialCanva.SetActive(true);
        TutorialTxt.text = TutorialMessages[1];
        yield return new WaitUntil(() => DialogueCanva.activeInHierarchy == false);
        yield return null;
        yield return null;
        yield return null;
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
        Time.timeScale = 1.0f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
