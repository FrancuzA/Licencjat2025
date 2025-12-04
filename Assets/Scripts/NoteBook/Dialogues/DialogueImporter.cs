using Mono.Cecil;
using System;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, DialogueGraph.AssetExtension)]
public class DialogueImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var Graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);

        if(Graph == null)
            return;

        var startNode = Graph.GetNodes().OfType<StartNode>().FirstOrDefault();
        if(startNode == null) return;

        var runtimeAsset = ScriptableObject.CreateInstance<DialogueRuntimeGraph>();
        var nextNode = GetNextNode(startNode);

        while (nextNode != null)
        {
            var runtimeNodes = TranslateNodeToRuntimeNode(nextNode);
        }
        
    }

    private DialogueRuntimeNodes TranslateNodeToRuntimeNode(INode nextNode)
    {
        return null;
    }

    static INode GetNextNode(INode currentNode)
    {
        var outputPort = currentNode.GetOutputPortByName(DialogueGraph.DefaultOutputName);
        var nextNodePort = outputPort.firstConnectedPort;
        var nextNode = nextNodePort?.GetNode();
        return nextNode;
    }

}
