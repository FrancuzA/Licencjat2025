using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
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
        var graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
        if (graph == null) return;


        var startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
        if (startNodeModel == null) return;

        var runtimeAsset = ScriptableObject.CreateInstance<DialogueRuntimeGraph>();
        runtimeAsset.StartingNode = TranslateNodeModelToRuntimeNodes(startNodeModel.GetOutputPort(0).firstConnectedPort.GetNode());

        ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
        ctx.SetMainObject(runtimeAsset);
    }

    private DialogueRuntimeNodes TranslateNodeModelToRuntimeNodes(INode nextNodeModel)
    {
        DialogueRuntimeNodes node = null;
        switch (nextNodeModel)
        {
            case MessageNode messageNode:
                messageNode.GetNodeOptionByName(messageNode.actorField).TryGetValue(out Actor actor);
                messageNode.GetNodeOptionByName(messageNode.msgField).TryGetValue(out string msg);
                node = new MessageRuntimeNode()
                {
                    _actor = actor,
                    message = msg,
                };
                break;
            case ChoiceNode choiceNode:
                choiceNode.GetNodeOptionByName(choiceNode.choiceAsset).TryGetValue(out GameChoice gameChoice);

                node = new ChoiceRuntimeNode()
                {
                    ChoiceAsset = gameChoice,
                };
                break;
        }

        foreach (var outputPort in nextNodeModel.GetOutputPorts())
        {
            if (!outputPort.isConnected) continue;
            node.OutputPorts.Add(TranslateNodeModelToRuntimeNodes(outputPort.firstConnectedPort.GetNode()));
        }

        return node;
    }

    static INode GetNextNode(INode currentNode)
    {
        var outputPorts = currentNode.GetOutputPorts();

        if (outputPorts.Count() == 1)
        {
            var outputPort = currentNode.GetOutputPortByName(DialogueGraph.DefaultOutputPortName);
            var nextNodePort = outputPort.firstConnectedPort;
            return nextNodePort?.GetNode();
        }

        if (outputPorts.Count() > 1)
        {

        }

        return null;
    }

}
