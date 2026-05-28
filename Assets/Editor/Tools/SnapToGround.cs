using UnityEngine;
using UnityEditor;

public static class SnapToGround
{
    [MenuItem("Tools/Snap To Ground %END")] // Ctrl+End (or use just "END" for plain End key)
    static void Snap()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponentInChildren<Renderer>();
            float offset = renderer != null
                ? obj.transform.position.y - renderer.bounds.min.y
                : 0f;

            Ray ray = new Ray(obj.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Undo.RecordObject(obj.transform, "Snap To Ground");
                obj.transform.position = new Vector3(
                    obj.transform.position.x,
                    hit.point.y + offset,
                    obj.transform.position.z
                );
            }
        }
    }
}