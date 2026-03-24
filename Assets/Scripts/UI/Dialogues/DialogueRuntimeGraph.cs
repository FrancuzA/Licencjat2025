
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueRuntimeGraph : ScriptableObject
{
    [SerializeReference] public DialogueRuntimeNodes StartingNode;
}
