using UnityEngine;

public class SecondNPCScript : MonoBehaviour
{
    private Animator npcAnim;


    private void Start()
    {
        npcAnim = GetComponent<Animator>();
    }

    public void StartPostDialogueAnimation()
    {
        npcAnim.SetTrigger("Point");
    }
}
