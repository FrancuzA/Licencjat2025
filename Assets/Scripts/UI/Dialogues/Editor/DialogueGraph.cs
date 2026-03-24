using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;


    [Graph(AssetExtension)]
    [Serializable]
    public class DialogueGraph : Graph
    {
        public const string AssetExtension = "dial";
        public const string DefaultOutputPortName = "Output";

        [MenuItem("Assets/Create/Graph Toolkit Samples/Dialogue Graph", false)]
        static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
        }
    }
