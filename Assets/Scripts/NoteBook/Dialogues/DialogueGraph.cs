using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

[Graph(AssetExtension)]
[Serializable]
class DialogueGraph : Graph
{
    public const string AssetExtension = "simpleg";
    public const string DefaultOutputName = "Output";

    [MenuItem("Assets/Create/Graph Toolkit Samples/Simple Graph", false)]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
    }
}

