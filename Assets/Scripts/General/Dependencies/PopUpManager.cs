using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private IEnumerator PopUpEvent(string messgage)
    {
        GameObject newPopUp = Instantiate(popUpPref, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        newPopUp.GetComponentInChildren<TextMeshProUGUI>().text = messgage;
        newPopUp.GetComponent<Animator>().SetTrigger("In");
        yield return new WaitForSecondsRealtime(10);
        newPopUp.GetComponent<Animator>().SetTrigger("Out");
        yield return new WaitForSeconds(2);
        Destroy(newPopUp);
    }
}
