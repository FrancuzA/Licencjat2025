using System.Collections.Generic;
using UnityEngine;

public class PlantGeneration : MonoBehaviour
{
    [Header("Mesh i material")]
    public Mesh _mesh;
    public Material material;
    public float meshScale;

    [Header("Obszar generacji (4 rogi czworok¹ta, XZ)")]
    public List<Vector3> corners = new List<Vector3>(4);

    [Header("Parametry siatki")]
    public float spacing = 1f;                  
    public float jitterAmount = 0.2f;

    [Header("Raycast")]
    public float rayLength = 100f;              
    public bool raycastDirectionUp = false;
    public LayerMask groundLayer = 1;   

    [Header("Automatyczne odœwie¿anie")]
    public bool regenerateOnUpdate;

    private List<Matrix4x4> instances = new List<Matrix4x4>();

    private void Awake()
    {
        Custom_RNG.Init(-1);
    }

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        if (regenerateOnUpdate)
        {
            instances.Clear();
            Generate();
        }

        if (_mesh != null && _mesh != null && material != null && instances.Count > 0)
        {
            Graphics.DrawMeshInstanced(_mesh, 0, material, instances.ToArray());
        }
    }

    [ContextMenu("Generuj")]
    public void Generate()
    {
        if (corners.Count < 3)
        {
            Debug.LogWarning("Musisz podaæ conajmniej 3 rogi n-k¹ta.");
            return;
        }

        instances.Clear();

       
        Bounds bounds = new Bounds(corners[0], Vector3.zero);
        foreach (var c in corners)
            bounds.Encapsulate(c);

        Debug.Log($"Bounding box: min=({bounds.min.x}, {bounds.min.y}, {bounds.min.z}), max=({bounds.max.x}, {bounds.max.y}, {bounds.max.z})");

        
        float rayStartY = transform.position.y;

        int candidateCount = 0;
        int insideCount = 0;
        int hitCount = 0;

        for (float x = bounds.min.x; x <= bounds.max.x; x += spacing)
        {
            for (float z = bounds.min.z; z <= bounds.max.z; z += spacing)
            {
                candidateCount++;
                Vector3 candidate = new Vector3(x, 0, z);

                if (!IsPointInPolygon(candidate))
                    continue;

                insideCount++;

                
                Vector3 rayStart = new Vector3(x, rayStartY, z);
                Vector3 rayDirection = raycastDirectionUp ? Vector3.up : Vector3.down;

                if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, rayLength, groundLayer))
                {
                    hitCount++;

                    // Dodaj losowe przesuniêcie w X i Z
                    float jitterX = Custom_RNG.Range(-jitterAmount, jitterAmount);
                    float jitterZ = Custom_RNG.Range(-jitterAmount, jitterAmount);

                    Vector3 finalPos = hit.point + new Vector3(jitterX, 0, jitterZ);
                    Quaternion rot = Quaternion.identity; 
                    Vector3 scale =new Vector3(meshScale,meshScale,meshScale);  

                    instances.Add(Matrix4x4.TRS(finalPos, rot, scale));
                }
            }
        }

        Debug.Log($"Kandydatów: {candidateCount}, wewn¹trz: {insideCount}, trafieñ: {hitCount}, instancji: {instances.Count}");
    }
     
    private bool IsPointInPolygon(Vector3 point)
    {
        int j = corners.Count - 1;
        bool inside = false;

        for (int i = 0; i < corners.Count; i++)
        {
            if ((corners[i].z > point.z) != (corners[j].z > point.z) &&
                (point.x < (corners[j].x - corners[i].x) * (point.z - corners[i].z) / (corners[j].z - corners[i].z) + corners[i].x))
            {
                inside = !inside;
            }
            j = i;
        }
        return inside;
    }
}