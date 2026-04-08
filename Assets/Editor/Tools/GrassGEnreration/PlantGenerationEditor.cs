using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PlantGeneration))]
public class PlantGenerationEditor : Editor
{
    private PlantGeneration script;

    private void OnEnable()
    {
        script = (PlantGeneration)target;
    }

    private void OnSceneGUI()
    {
        if (script == null || script.corners == null || script.corners.Count == 0)
            return;

        Handles.color = script.polygonColor;
        for (int i = 0; i < script.corners.Count; i++)
        {
            int next = (i + 1) % script.corners.Count;
            Handles.DrawLine(script.corners[i], script.corners[next]);
        }

        for (int i = 0; i < script.corners.Count; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(script.corners[i], Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "Move polygon point");
                newPos.y = 0f; // wymuœ Y=0
                script.corners[i] = newPos;
                if (script.regenerateOnUpdate)
                    script.Generate();
                else
                    EditorUtility.SetDirty(script);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        if (GUILayout.Button("Generuj teraz"))
        {
            script.Generate();
        }

        if (GUILayout.Button("Resetuj listê punktów (kwadrat 10x10)"))
        {
            Undo.RecordObject(script, "Reset polygon points");
            script.corners = new List<Vector3>
            {
                new Vector3(-5, 0, -5),
                new Vector3( 5, 0, -5),
                new Vector3( 5, 0,  5),
                new Vector3(-5, 0,  5)
            };
            EditorUtility.SetDirty(script);
        }

        if (GUILayout.Button("Wyrównaj wszystkie punkty do Y=0"))
        {
            Undo.RecordObject(script, "Set Y to zero");
            script.SetCornersYZero();
            EditorUtility.SetDirty(script);
        }
    }
}