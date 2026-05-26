using UnityEngine;

public class SecondNPCScript : MonoBehaviour
{
    private Animator npcAnim;

    public GameObject eventIndicator;
    public string triggerName;

    private void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    public void StartPostDialogueAnimation()
    {
        eventIndicator.SetActive(false);
        npcAnim.SetTrigger(triggerName);
    }
}
