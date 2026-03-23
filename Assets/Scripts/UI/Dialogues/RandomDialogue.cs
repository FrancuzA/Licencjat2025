using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomDialogue", menuName = "DialogueSystem/RandomDialogue")]
public class RandomDialogue : ScriptableObject
{
    [field: SerializeField] public List<string> Dialogues { get; private set; } = new();
}
