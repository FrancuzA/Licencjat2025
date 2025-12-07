using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MessageRuntimeNode : DialogueRuntimeNodes
{
    public Actor _actor;
    public string message;
    public Sprite avatar;
}
