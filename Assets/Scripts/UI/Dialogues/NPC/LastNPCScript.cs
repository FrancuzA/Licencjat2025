using UnityEngine;

public class LastNPCScript : MonoBehaviour
{
    private Dependencies _dep;
    private CutSceneManager _cutScene;
    private NPCScript _npc;
    private bool progressFinished;

    public DialogueRuntimeGraph normalDialogue;
    public DialogueRuntimeGraph endDialogue;
    void Start()
    {
        _dep = Dependencies.Instance;
        _cutScene = _dep.GetDependancy<CutSceneManager>();
        _npc = GetComponent<NPCScript>();
    }

    public void CheckProgress()
    {
        if (_cutScene.correctWords < 3)
        {

            _npc.NPCDialogue = normalDialogue;
        }
        else
        {
            _npc.NPCDialogue = endDialogue;
            progressFinished = true;
        }

    }

    public void TryStartEndCutScene()
    {
        if (progressFinished) _cutScene.StartEndCutScene();
    }
}
