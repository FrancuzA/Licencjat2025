
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueRuntimeGraph : ScriptableObject
{
    public List<DialogueRuntimeNodes> nodes = new();
}
