using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCP Preset Data", menuName = "Scene Camera Positioner/Preset Data")]
public class SCP_SaveData : ScriptableObject
{
    [SerializeField] public List<SCP_PresetItem> _presets;

}
