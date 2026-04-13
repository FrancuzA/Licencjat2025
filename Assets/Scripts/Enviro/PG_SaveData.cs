using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PG Region Data", menuName = "Plant Generation/Region Data")]
public class PG_SaveData : ScriptableObject
{
    [SerializeField] public List<PG_RegionItem> _regions = new();
}
