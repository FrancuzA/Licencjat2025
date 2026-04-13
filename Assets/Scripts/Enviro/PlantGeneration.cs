using System.Collections.Generic;
using UnityEngine;

public class PlantGeneration : MonoBehaviour
{
    [Header("Automatyczne odœwie¿anie")]
    public bool regenerateOnUpdate;

    [HideInInspector]
    public PG_SaveData saveData;

    public float rayLength = 100f;
    public bool raycastDirectionUp = false;

    private class RegionData
    {
        public Mesh mesh;
        public Material material;
        public float meshScale;
        public List<Vector3> corners;
        public float spacing;
        public float jitterAmount;
        public LayerMask groundLayer;
        public List<Matrix4x4> instances = new List<Matrix4x4>();
    }

    private Dictionary<int, RegionData> regionDataMap = new Dictionary<int, RegionData>();

    private void Awake()
    {
        Custom_RNG.Init(-1);
    }

    private void Start()
    {
        if (saveData != null)
            GenerateAll();
    }

    private void Update()
    {
        if (regenerateOnUpdate)
            GenerateAll();

        foreach (var kvp in regionDataMap)
        {
            var data = kvp.Value;
            if (data.mesh != null && data.material != null && data.instances.Count > 0)
                Graphics.DrawMeshInstanced(data.mesh, 0, data.material, data.instances.ToArray());
        }
    }

    public void GenerateAll()
    {
        if (saveData == null) return;
        for (int i = 0; i < saveData._regions.Count; i++)
            Generate(i);
    }

    public void Generate(int index)
    {
        if (saveData == null || index < 0 || index >= saveData._regions.Count) return;

        var region = saveData._regions[index];
        if (region.corners.Count < 3) return;

        if (!regionDataMap.ContainsKey(index))
            regionDataMap[index] = new RegionData();

        var data = regionDataMap[index];

        // Kopiuj dane
        data.mesh = region.mesh;
        data.material = region.material;
        data.meshScale = region.meshScale;
        data.corners = region.corners;
        data.spacing = region.spacing;
        data.jitterAmount = region.jitterAmount;
        data.groundLayer = region.groundLayer;

        data.instances.Clear();

        Bounds bounds = new Bounds(data.corners[0], Vector3.zero);
        foreach (var c in data.corners) bounds.Encapsulate(c);

        float rayStartY = transform.position.y;

        for (float x = bounds.min.x; x <= bounds.max.x; x += data.spacing)
        {
            for (float z = bounds.min.z; z <= bounds.max.z; z += data.spacing)
            {
                Vector3 candidate = new Vector3(x, 0, z);
                if (!IsPointInPolygon(candidate, data.corners)) continue;

                Vector3 rayStart = new Vector3(x, rayStartY, z);
                Vector3 rayDir = raycastDirectionUp ? Vector3.up : Vector3.down;

                if (Physics.Raycast(rayStart, rayDir, out RaycastHit hit, rayLength, data.groundLayer))
                {
                    float jitterX = Custom_RNG.Range(-data.jitterAmount, data.jitterAmount);
                    float jitterZ = Custom_RNG.Range(-data.jitterAmount, data.jitterAmount);
                    Vector3 finalPos = hit.point + new Vector3(jitterX, 0, jitterZ);
                    Vector3 scale = Vector3.one * data.meshScale;
                    data.instances.Add(Matrix4x4.TRS(finalPos, Quaternion.identity, scale));
                }
            }
        }

    }

    private bool IsPointInPolygon(Vector3 point, List<Vector3> corners)
    {
        int j = corners.Count - 1;
        bool inside = false;
        for (int i = 0; i < corners.Count; i++)
        {
            if ((corners[i].z > point.z) != (corners[j].z > point.z) &&
                (point.x < (corners[j].x - corners[i].x) * (point.z - corners[i].z) / (corners[j].z - corners[i].z) + corners[i].x))
                inside = !inside;
            j = i;
        }
        return inside;
    }

    public void ClearAll() => regionDataMap.Clear();
}