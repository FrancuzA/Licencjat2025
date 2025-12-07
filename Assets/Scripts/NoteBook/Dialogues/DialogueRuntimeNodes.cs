
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DialogueRuntimeNodes
{
    [SerializeReference] public List<DialogueRuntimeNodes> OutputPorts = new();

}