using System.Collections.Generic;
using UnityEngine;

public class ArmyGenerator : MonoBehaviour
{
    [Header("Mesh i material")]
    public List<Mesh> meshes = new();
    public Material material;

    [Header("Obszar generacji (4 rogi czworok¹ta, XZ)")]
    public List<Vector3> corners = new List<Vector3>(4);

    [Header("Parametry siatki")]
    public float spacing = 1f;
    public float jitterAmount = 0.2f;

    [Header("Raycast")]
    public float rayLength = 2000f;
    public bool raycastDirectionUp = false;  // false = w dó³, true = w górê
    public LayerMask groundLayer = 1;

    [Header("Automatyczne odœwie¿anie")]
    public bool regenerateOnUpdate;

    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColorInside = Color.green;
    public Color gizmoColorOutside = Color.red;

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

        if (meshes != null && meshes.Count > 0 && material != null && instances.Count > 0)
        {
            Graphics.DrawMeshInstanced(meshes[0], 0, material, instances.ToArray());
        }
    }

    [ContextMenu("Generuj")]
    public void Generate()
    {
        if (corners.Count < 4)
        {
            Debug.LogWarning("Musisz podaæ dok³adnie 4 rogi czworok¹ta.");
            return;
        }

        instances.Clear();

        // Wyznacz bounding box czworok¹ta (tylko X i Z)
        Bounds bounds = new Bounds(new Vector3(corners[0].x, 0, corners[0].z), Vector3.zero);
        foreach (var c in corners)
            bounds.Encapsulate(new Vector3(c.x, 0, c.z));

        Debug.Log($"Bounding box: min={bounds.min}, max={bounds.max}");

        int totalCandidates = 0;
        int insideCount = 0;
        int hitCount = 0;

        // Iteruj po punktach siatki w bounding boxie z krokiem spacing
        for (float x = bounds.min.x; x <= bounds.max.x + 0.01f; x += spacing)
        {
            for (float z = bounds.min.z; z <= bounds.max.z + 0.01f; z += spacing)
            {
                totalCandidates++;
                Vector3 candidate = new Vector3(x, 0, z);

                // SprawdŸ czy punkt le¿y wewn¹trz czworok¹ta
                if (!IsPointInPolygon(candidate))
                    continue;

                insideCount++;

                // Wykonaj raycast
                Vector3 rayStart;
                Vector3 rayDirection;

                if (raycastDirectionUp)
                {
                    rayStart = new Vector3(x, -1000f, z); // start nisko, strzelamy w górê
                    rayDirection = Vector3.up;
                }
                else
                {
                    rayStart = new Vector3(x, 1000f, z); // start wysoko, strzelamy w dó³
                    rayDirection = Vector3.down;
                }

                if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, rayLength, groundLayer))
                {
                    hitCount++;
                    // Dodaj losowe przesuniêcie w X i Z
                    float jitterX = Custom_RNG.Range(-jitterAmount, jitterAmount);
                    float jitterZ = Custom_RNG.Range(-jitterAmount, jitterAmount);

                    Vector3 finalPos = hit.point + new Vector3(jitterX, 0, jitterZ);
                    Quaternion rot = Quaternion.identity;
                    Vector3 scale = Vector3.one;

                    instances.Add(Matrix4x4.TRS(finalPos, rot, scale));
                }
            }
        }

        Debug.Log($"Kandydatów: {totalCandidates}, wewn¹trz: {insideCount}, trafieñ: {hitCount}, instancji: {instances.Count}");
    }

    // Algorytm point-in-polygon (ray casting)
    private bool IsPointInPolygon(Vector3 point)
    {
        int j = corners.Count - 1;
        bool inside = false;

        for (int i = 0; i < corners.Count; i++)
        {
            Vector3 vi = corners[i];
            Vector3 vj = corners[j];

            // SprawdŸ czy odcinek (vi, vj) przecina pó³prost¹ poziom¹ w prawo z punktu point
            if (((vi.z > point.z) != (vj.z > point.z)) &&
                (point.x < (vj.x - vi.x) * (point.z - vi.z) / (vj.z - vi.z) + vi.x))
            {
                inside = !inside;
            }
            j = i;
        }
        return inside;
    }

    // Rysowanie Gizmos dla wizualizacji
    private void OnDrawGizmosSelected()
    {
        if (!showGizmos || corners.Count < 4) return;

        // Rysuj czworok¹t
        Gizmos.color = Color.cyan;
        for (int i = 0; i < corners.Count; i++)
        {
            Vector3 a = corners[i];
            Vector3 b = corners[(i + 1) % corners.Count];
            Gizmos.DrawLine(a, b);
        }

        // Rysuj punkty siatki (zaznacz wewnêtrzne/zewnêtrzne)
        Bounds bounds = new Bounds(new Vector3(corners[0].x, 0, corners[0].z), Vector3.zero);
        foreach (var c in corners)
            bounds.Encapsulate(new Vector3(c.x, 0, c.z));

        for (float x = bounds.min.x; x <= bounds.max.x + 0.01f; x += spacing)
        {
            for (float z = bounds.min.z; z <= bounds.max.z + 0.01f; z += spacing)
            {
                Vector3 candidate = new Vector3(x, 0, z);
                bool inside = IsPointInPolygon(candidate);
                Gizmos.color = inside ? gizmoColorInside : gizmoColorOutside;
                Gizmos.DrawSphere(candidate, 0.1f);
            }
        }
    }
}