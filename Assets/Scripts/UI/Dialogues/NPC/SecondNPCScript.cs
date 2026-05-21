using UnityEngine;

public class SecondNPCScript : MonoBehaviour
{
    private Animator npcAnim;

    public string triggerName;

    private void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    public void StartPostDialogueAnimation()
    {
        npcAnim.SetTrigger(triggerName);
    }
}
