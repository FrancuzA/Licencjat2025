using UnityEditor;
using UnityEngine;

public class PrefabBrushTool : EditorWindow
{
    GameObject prefab;

    float brushSize = 2f;
    float spacing = 1f;
    int density = 5;

    float yOffset = 0f;
    Vector2 randomRotationX = new Vector2(0, 0);
    Vector2 randomRotationY = new Vector2(0, 360);
    Vector2 randomRotationZ = new Vector2(0, 0);
    Vector2 randomScale = new Vector2(0.8f, 1.3f);

    Transform parentTransform;

    float lastPaintTime;

    [MenuItem("Tools/Prefab Brush")]
    static void Open() => GetWindow<PrefabBrushTool>("Prefab Brush");

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        EditorGUILayout.LabelField("Brush Settings", EditorStyles.boldLabel);
        brushSize = EditorGUILayout.Slider("Brush Size", brushSize, 0.5f, 10f);
        spacing = EditorGUILayout.Slider("Spacing", spacing, 0.05f, 5f);
        density = EditorGUILayout.IntSlider("Density", density, 1, 20);

        EditorGUILayout.LabelField("Prefab Transform", EditorStyles.boldLabel);
        yOffset = EditorGUILayout.Slider("Y Offset", yOffset, -5f, 5f);
        randomRotationX = EditorGUILayout.Vector2Field("Random X Rotation", randomRotationX);
        randomRotationY = EditorGUILayout.Vector2Field("Random Y Rotation", randomRotationY);
        randomRotationZ = EditorGUILayout.Vector2Field("Random Z Rotation", randomRotationZ);
        randomScale = EditorGUILayout.Vector2Field("Random Scale Range", randomScale);

        parentTransform = (Transform)EditorGUILayout.ObjectField("Parent Object", parentTransform, typeof(Transform), true);

        EditorGUILayout.HelpBox("LPM – maluj po colliderach. Trzymaj LPM, aby malować ciągle.", MessageType.Info);
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (prefab == null) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(hit.point, hit.normal, brushSize);

            if ((e.type == EventType.MouseDown || (e.type == EventType.MouseDrag && e.button == 0)))
            {
                if (EditorApplication.timeSinceStartup - lastPaintTime > spacing * 0.1f)
                {
                    for (int i = 0; i < density; i++)
                    {
                        Vector2 randomCircle = Random.insideUnitCircle * brushSize;
                        Vector3 spawnXZ = hit.point + new Vector3(randomCircle.x, 100f, randomCircle.y); // wysoki punkt startowy

                        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                        // Losowa skala
                        float scale = Random.Range(randomScale.x, randomScale.y);
                        go.transform.localScale *= scale;

                        // Losowa rotacja
                        float rotX = Random.Range(randomRotationX.x, randomRotationX.y);
                        float rotY = Random.Range(randomRotationY.x, randomRotationY.y);
                        float rotZ = Random.Range(randomRotationZ.x, randomRotationZ.y);
                        go.transform.rotation = prefab.transform.rotation * Quaternion.Euler(rotX, rotY, rotZ);

                        // Raycast w dół od góry prefabru, żeby trafił w podłoże
                        Ray downRay = new Ray(spawnXZ + Vector3.up * 50f, Vector3.down);
                        if (Physics.Raycast(downRay, out RaycastHit groundHit, 200f))
                        {
                            go.transform.position = groundHit.point + new Vector3(0, yOffset, 0);
                        }
                        else
                        {
                            // fallback jeśli nie trafia w ziemię
                            go.transform.position = spawnXZ + new Vector3(0, yOffset, 0);
                        }

                        if (parentTransform != null)
                            go.transform.parent = parentTransform;

                        Undo.RegisterCreatedObjectUndo(go, "Paint Prefab");
                    }

                    lastPaintTime = (float)EditorApplication.timeSinceStartup;
                }

                e.Use();
            }
        }
    }

    void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;
}
