using UnityEngine;
using UnityEditor;

public class FixPinkMaterials : MonoBehaviour
{
    [MenuItem("Tools/Fix All Pink Materials")]
    static void FixAllPinkMaterials()
    {
        // Find all materials in the project
        string[] materialGUIDs = AssetDatabase.FindAssets("t:Material");
        int fixedCount = 0;

        foreach (string guid in materialGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            // Check if material is pink (missing shader or error shader)
            if (material.shader.name.Contains("Error") ||
                material.shader.name == null ||
                material.shader.name == "" ||
                material.shader.name.Contains("Hidden/InternalErrorShader"))
            {
                Debug.Log($"Fixing pink material: {path}");

                // Try to assign an appropriate HDRP shader based on material name
                Shader newShader = GetAppropriateShader(material.name);

                if (newShader != null)
                {
                    material.shader = newShader;
                    EditorUtility.SetDirty(material);
                    fixedCount++;
                }
                else
                {
                    Debug.LogWarning($"Could not find appropriate shader for: {material.name}");
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Fixed {fixedCount} pink materials!");
    }

    static Shader GetAppropriateShader(string materialName)
    {
        // Common shader mappings - adjust based on your needs
        string[] waterKeywords = { "water", "Water", "WATER", "ocean", "sea", "river" };
        string[] transparentKeywords = { "glass", "window", "transparent", "alpha" };
        string[] vegetationKeywords = { "tree", "leaf", "grass", "plant", "foliage" };
        string[] metalKeywords = { "metal", "chrome", "steel", "iron" };

        materialName = materialName.ToLower();

        // Check for water materials
        foreach (string keyword in waterKeywords)
        {
            if (materialName.Contains(keyword))
            {
                Shader shader = Shader.Find("HDRP/Water");
                if (shader != null) return shader;

                shader = Shader.Find("HDRP/Lit");
                return shader;
            }
        }

        // Check for transparent materials
        foreach (string keyword in transparentKeywords)
        {
            if (materialName.Contains(keyword))
            {
                Shader shader = Shader.Find("HDRP/LitTransparent");
                if (shader != null) return shader;
            }
        }

        // Check for vegetation
        foreach (string keyword in vegetationKeywords)
        {
            if (materialName.Contains(keyword))
            {
                Shader shader = Shader.Find("HDRP/Lit");
                return shader;
            }
        }

        // Default to HDRP/Lit
        return Shader.Find("HDRP/Lit");
    }
}