using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlantGeneration))]
public class PlantGenerationEditor : Editor
{
    private PlantGeneration script;

    private void OnEnable() => script = (PlantGeneration)target;

    private void OnSceneGUI()
    {
        if (script == null || script.saveData == null || script.saveData._regions == null)
            return;

        for (int r = 0; r < script.saveData._regions.Count; r++)
        {
            var region = script.saveData._regions[r];
            if (region.corners == null || region.corners.Count < 3) continue;

            // Rysuj linie
            Handles.color = region.polygonColor;
            for (int i = 0; i < region.corners.Count; i++)
            {
                int next = (i + 1) % region.corners.Count;
                Handles.DrawLine(region.corners[i], region.corners[next]);
            }

            // Rysuj uchwyty
            for (int i = 0; i < region.corners.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPos = Handles.PositionHandle(region.corners[i], Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(script.saveData, $"Move {region.regionName} corner");
                    newPos.y = 0f;
                    region.corners[i] = newPos;
                    EditorUtility.SetDirty(script.saveData);
                    script.Generate(r);  // regeneruj tylko ten region
                    SceneView.RepaintAll();
                }
            }
        }
    }
}