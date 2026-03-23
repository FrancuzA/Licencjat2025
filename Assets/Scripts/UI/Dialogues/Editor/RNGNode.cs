using System;
using System.Collections.Generic;
using DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine.Serialization;

namespace DialogueSystem.Editor.Nodes
{
    [Serializable]
    public class RNGNode : Node
    {
        public string rngAsset = "rngAsset";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<RandomDialogue>(rngAsset).WithDisplayName("RandomDialogues").Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("Input").Build();

            if (GetNodeOptionByName(rngAsset).TryGetValue(out RandomDialogue rDIalogues))
            {
                if (rDIalogues == null)
                    return;
                for (int i = 0; i < rDIalogues.Dialogues.Count; i++)
                {
                    context.AddOutputPort($"Dialogue{i}").WithDisplayName($"{rDIalogues.Dialogues[i]}").Build();
                }
            }
        }
    }
}
