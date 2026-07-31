#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Creates and activates the project's 2D URP assets, then runs Unity's
/// Built-in 2D to URP 2D material/reference converter.
/// </summary>
public static class MadCountsUrpMigration
{
    private const string SettingsFolder = "Assets/_Project/Settings";
    private const string UrpFolder = SettingsFolder + "/URP";
    private const string RendererPath = UrpFolder + "/MadCounts_Renderer2D.asset";
    private const string PipelinePath = UrpFolder + "/MadCounts_URP.asset";
    private const string QualityAssetFolder = "Assets/URPDefaultResources";
    private static readonly string[] QualityAssetNames =
    {
        "Very Low", "Low", "Medium", "High", "Very High", "Ultra"
    };

    [MenuItem("Tools/MadCounts/Convert Built-in 2D to URP 2D")]
    public static void ConvertBuiltIn2DToUrp2D()
    {
        EnsureFolder("Assets/_Project", "Settings");
        EnsureFolder(SettingsFolder, "URP");

        var rendererData = AssetDatabase.LoadAssetAtPath<Renderer2DData>(RendererPath);
        if (rendererData == null)
        {
            rendererData = ScriptableObject.CreateInstance<Renderer2DData>();
            rendererData.name = "MadCounts_Renderer2D";
            AssetDatabase.CreateAsset(rendererData, RendererPath);
        }

        var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);
        if (pipelineAsset == null)
        {
            pipelineAsset = UniversalRenderPipelineAsset.Create(rendererData);
            pipelineAsset.name = "MadCounts_URP";
            AssetDatabase.CreateAsset(pipelineAsset, PipelinePath);
        }

        GraphicsSettings.defaultRenderPipeline = pipelineAsset;
        EditorUtility.SetDirty(rendererData);
        EditorUtility.SetDirty(pipelineAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // The 2D container updates built-in sprite materials and material
        // references in scenes/prefabs. Avoid re-running it once its quality
        // assets exist: Unity's 17.5.0 batch converter reports those assets as
        // errors on a second pass even though the project is already converted.
        if (!HasConvertedQualityAssets())
        {
#pragma warning disable CS0618 // Unity 6.5's 17.5.0 converter still exposes the stable enum entry point.
            Converters.RunInBatchMode(ConverterContainerId.BuiltInToURP2D);
#pragma warning restore CS0618
        }
        else
        {
            Debug.Log("MadCounts URP migration already applied; skipping the converter pass.");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"MadCounts URP migration complete. Active asset: {PipelinePath}");
    }

    [MenuItem("Tools/MadCounts/Validate URP 2D Migration")]
    public static void ValidateUrp2DMigration()
    {
        var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);
        var rendererData = AssetDatabase.LoadAssetAtPath<Renderer2DData>(RendererPath);
        var activePipeline = GraphicsSettings.defaultRenderPipeline;
        var qualityPipeline = QualitySettings.renderPipeline;

        if (pipelineAsset == null)
        {
            throw new System.InvalidOperationException($"Missing URP pipeline asset: {PipelinePath}");
        }

        if (rendererData == null)
        {
            throw new System.InvalidOperationException($"Missing 2D renderer asset: {RendererPath}");
        }

        if (activePipeline != pipelineAsset)
        {
            throw new System.InvalidOperationException(
                $"Active render pipeline does not match {PipelinePath}. Current: {activePipeline}");
        }

        if (qualityPipeline != null && !(qualityPipeline is UniversalRenderPipelineAsset))
        {
            throw new System.InvalidOperationException(
                $"Current quality level is using a non-URP pipeline: {qualityPipeline}");
        }

        Debug.Log(
            $"MADCOUNTS_URP_VALIDATION: PASS (pipeline={PipelinePath}, renderer={RendererPath}, " +
            $"qualityPipeline={qualityPipeline})");
    }

    private static void EnsureFolder(string parent, string child)
    {
        var path = parent + "/" + child;
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(parent, child);
        }
    }

    private static bool HasConvertedQualityAssets()
    {
        foreach (var qualityAssetName in QualityAssetNames)
        {
            var path = $"{QualityAssetFolder}/{qualityAssetName}.asset";
            if (AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(path) == null)
            {
                return false;
            }
        }

        return true;
    }
}
#endif
