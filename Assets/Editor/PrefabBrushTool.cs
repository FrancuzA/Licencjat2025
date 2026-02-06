using UnityEditor;
using UnityEngine;

public class PrefabBrushTool : EditorWindow
{
    GameObject prefab;
    float brushSize = 2f;
    float spacing = 1f;
    float lastPaintTime;

    [MenuItem("Tools/Prefab Brush")]
    static void Open()
    {
        GetWindow<PrefabBrushTool>("Prefab Brush");
    }

    void OnGUI()
    {
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        brushSize = EditorGUILayout.Slider("Brush Size", brushSize, 0.5f, 10f);
        spacing = EditorGUILayout.Slider("Spacing", spacing, 0.1f, 5f);

        EditorGUILayout.HelpBox("LPM – maluj po colliderach", MessageType.Info);
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

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (EditorApplication.timeSinceStartup - lastPaintTime > spacing * 0.1f)
                {
                    Vector3 pos = hit.point + hit.normal * 0.01f;
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                    go.transform.position = pos;
                    go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    go.transform.localScale *= Random.Range(0.8f, 1.3f);

                    Undo.RegisterCreatedObjectUndo(go, "Paint Prefab");
                    lastPaintTime = (float)EditorApplication.timeSinceStartup;
                }
                e.Use();
            }
        }
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
