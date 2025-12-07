using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Editor.Nodes;
using DialogueSystem.Runtime.Nodes;
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
            var runtimeNodes = TranslateNodeModelToRuntimeNodes(nextNode);
            runtimeAsset.Nodes.Add(runtimeNodes);

            nextNode = GetNextNode(nextNode);
        }

        ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
        ctx.SetMainObject(runtimeAsset);
    }

    private DialogueRuntimeNodes TranslateNodeToRuntimeNode(INode nextNode)
    {
        DialogueRuntimeNode node = null;
        switch (nextNode)
        {
            case MessageNode messageNode:
                messageNode.GetNodeOptionByName(messageNode.actorField).TryGetValue(out Actor actor);
                messageNode.GetNodeOptionByName(messageNode.msgField).TryGetValue(out string msg);
                node = new MessageRuntimeNode()
                {
                    Actor = actor,
                    Message = msg,
                };
                break;
        }

        return node;
    }

    static INode GetNextNode(INode currentNode)
    {
        var outputPort = currentNode.GetOutputPortByName(DialogueGraph.DefaultOutputName);
        var nextNodePort = outputPort.firstConnectedPort;
        var nextNode = nextNodePort?.GetNode();
        return nextNode;
    }

}
