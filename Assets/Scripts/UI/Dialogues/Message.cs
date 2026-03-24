using UnityEngine;

[CreateAssetMenu(fileName = "Message", menuName = "Dialogues/Message")]
public class Message : ScriptableObject
{
    [field: SerializeField] public Actor Actor {get; private set;}
    [field: SerializeField, TextArea(3, 10)] public string Content {get; private set;}


}
