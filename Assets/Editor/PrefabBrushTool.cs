using UnityEditor;
using UnityEngine;

public class PrefabBrushTool : EditorWindow
{
    GameObject prefab;

    float brushSize = 2f;
    float spacing = 0.1f;
    int density = 5;

    float yOffset = 0f;
    Vector2 randomRotX = Vector2.zero;
    Vector2 randomRotY = new Vector2(0, 360);
    Vector2 randomRotZ = Vector2.zero;
    Vector2 randomScale = new Vector2(1f, 1f);

    Transform parent;
    float lastPaintTime;

    [MenuItem("Tools/Prefab Brush")]
    static void Open() => GetWindow<PrefabBrushTool>("Prefab Brush");

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Brush", EditorStyles.boldLabel);
        brushSize = EditorGUILayout.Slider("Brush Size", brushSize, 0.5f, 20f);
        spacing = EditorGUILayout.Slider("Spacing (time)", spacing, 0.01f, 1f);
        density = EditorGUILayout.IntSlider("Density", density, 1, 50);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Transform", EditorStyles.boldLabel);
        yOffset = EditorGUILayout.Slider("Y Offset", yOffset, -5f, 5f);
        randomRotX = EditorGUILayout.Vector2Field("Random Rot X", randomRotX);
        randomRotY = EditorGUILayout.Vector2Field("Random Rot Y", randomRotY);
        randomRotZ = EditorGUILayout.Vector2Field("Random Rot Z", randomRotZ);
        randomScale = EditorGUILayout.Vector2Field("Random Scale", randomScale);

        parent = (Transform)EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true);

        EditorGUILayout.HelpBox(
            "LPM – paint\nPPM + WASD – normal camera movement",
            MessageType.Info
        );
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (prefab == null) return;

        Event e = Event.current;

        // 🔑 POZWÓL NA NORMALNĄ KAMERĘ
        if (e.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(
                GUIUtility.GetControlID(FocusType.Passive)
            );
        }

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        // Brush gizmo
        Handles.color = Color.green;
        Handles.DrawWireDisc(hit.point, hit.normal, brushSize);

        // TYLKO LPM
        if (e.button != 0) return;

        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            if (EditorApplication.timeSinceStartup - lastPaintTime < spacing)
                return;

            lastPaintTime = (float)EditorApplication.timeSinceStartup;

            for (int i = 0; i < density; i++)
            {
                Vector2 rnd = Random.insideUnitCircle * brushSize;
                Vector3 rayStart = hit.point + new Vector3(rnd.x, 50f, rnd.y);

                if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit ground, 200f))
                    continue;

                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                float scale = Random.Range(randomScale.x, randomScale.y);
                go.transform.localScale *= scale;

                Quaternion rot =
                    prefab.transform.rotation *
                    Quaternion.Euler(
                        Random.Range(randomRotX.x, randomRotX.y),
                        Random.Range(randomRotY.x, randomRotY.y),
                        Random.Range(randomRotZ.x, randomRotZ.y)
                    );

                go.transform.SetPositionAndRotation(
                    ground.point + Vector3.up * yOffset,
                    rot
                );

                if (parent != null)
                    go.transform.parent = parent;

                Undo.RegisterCreatedObjectUndo(go, "Paint Prefab");
            }

            // ❗ BLOKUJEMY TYLKO LPM
            e.Use();
        }
    }

    void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
    void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;
}
