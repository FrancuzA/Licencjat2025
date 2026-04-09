using System.Collections.Generic;
using UnityEngine;

public class PG_RegionItem 
{
    public string regionName;
    public Mesh mesh;
    public Material material;
    public float meshScale;
    public Color polygonColor = Color.red;
    public float spacing = 1f;
    public float jitterAmount = 0.2f;
    public LayerMask groundLayer = 1;
    public List<Vector3> corners;

}
