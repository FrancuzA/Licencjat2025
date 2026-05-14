using UnityEngine;
using UnityEditor;

public class FindMissingScriptsRecursively : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts In Project")]
    static void Find()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        int foundCount = 0;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                int missing = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(prefab);
                if (missing > 0)
                {
                    Debug.Log($"❌ Prefab '{path}' has {missing} missing script(s).", prefab);
                    foundCount++;
                }
            }
        }
        Debug.Log($"✅ Finished. Found {foundCount} prefabs with missing scripts.");
    }
}