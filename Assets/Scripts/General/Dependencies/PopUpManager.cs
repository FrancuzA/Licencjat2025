using System.Collections;
using TMPro;
using UnityEngine;
public class PopUpManager : MonoBehaviour
{
    public GameObject popUpPref;

    public void Start()
    {
        Dependencies.Instance.RegisterDependency<PopUpManager>(this);
    }

    public void StartPopUp(string message)
    {
        StartCoroutine(PopUpEvent(message));
    }
    public void StartHintPopUp(string hint)
    {
        StartCoroutine(HintEvent(hint));
    }

    private IEnumerator PopUpEvent(string message)
    {
        var fullMessage = $"{message} PRESS J TO OPEN THE JOURNAL";
        GameObject newPopUp = Instantiate(popUpPref, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        newPopUp.GetComponentInChildren<TextMeshProUGUI>().text = fullMessage;
        newPopUp.GetComponent<Animator>().SetTrigger("In");
        yield return new WaitForSecondsRealtime(10);
        newPopUp.GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSeconds(2);
        Destroy(newPopUp);
    }

    private IEnumerator HintEvent(string hint)
    {
       
        GameObject newPopUp = Instantiate(popUpPref, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        newPopUp.GetComponentInChildren<TextMeshProUGUI>().text = hint;
        newPopUp.GetComponent<Animator>().SetTrigger("In");
        yield return new WaitForSecondsRealtime(10);
        newPopUp.GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSeconds(2);
        Destroy(newPopUp);
    }
}
