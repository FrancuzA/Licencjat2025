using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Test : MonoBehaviour
{
    /*
    void Start()
    {
        Debug.Log("Applying HDRP compute shader fixes...");

        // Get the current HDRP asset
        var hdrpAsset = GraphicsSettings.currentRenderPipeline as HDRenderPipelineAsset;
        if (hdrpAsset != null)
        {
            Debug.Log($"Current HDRP Asset: {hdrpAsset.name}");

            // Try switching to a simpler HDRP asset temporarily
            SwitchToSimplerHDRPAsset();
        }
    }

    void SwitchToSimplerHDRPAsset()
    {
        // Look for simpler HDRP assets in your project
        // Usually there are multiple quality levels
        HDRenderPipelineAsset[] allHDRPAssets = Resources.FindObjectsOfTypeAll<HDRenderPipelineAsset>();

        foreach (var asset in allHDRPAssets)
        {
            if (asset.name.ToLower().Contains("low") || asset.name.ToLower().Contains("medium"))
            {
                //GraphicsSettings.currentRenderPipeline = asset;
                GraphicsSettings.defaultRenderPipeline = asset;
                Debug.Log($"Switched to simpler HDRP asset: {asset.name}");
                return;
            }
        }

        Debug.LogWarning("No alternative HDRP assets found. Creating temporary fix...");
        ApplyTemporaryHDRPFix();
    }

    void ApplyTemporaryHDRPFix()
    {
        // Disable compute-intensive features via code
        var hdCamera = GameObject.FindObjectOfType<HDAdditionalCameraData>();
        if (hdCamera != null)
        {
            // Try to disable features that might use compute shaders
            Debug.Log("Found HD Camera - attempting to disable compute features");
        }
    }
    */
}