using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData 
{
    public Vector3 playerPosition = Vector3.zero;
    public Dictionary<string, Vector3> NPCPositions;
}
