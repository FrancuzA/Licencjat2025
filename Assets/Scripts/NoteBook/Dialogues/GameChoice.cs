using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Choice", menuName = "DialogueSystem/Choice", order = 0)]
public class GameChoice : ScriptableObject
{
    [field: SerializeField] public List<string> Choices { get; private set; } = new();
}
