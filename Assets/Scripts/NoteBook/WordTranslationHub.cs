using UnityEngine;
using UnityEngine.Events;

public class WordTranslationHub : MonoBehaviour
{
    public UnityEvent FirstNPC;
    public UnityEvent Fisherman;
    public UnityEvent Herbalist;

    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<WordTranslationHub>(this);
    }

    public void NewWordTranslated(string word)
    {
        switch (word)
        {
            case "c": FirstNPC.Invoke();
                break;
            case "s": Fisherman.Invoke();
                break;
            case "p": Herbalist.Invoke();
                break;
            default: FirstNPC.Invoke();
                break;
        }
    }
}
