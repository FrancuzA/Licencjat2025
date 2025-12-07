using System;
using System.Collections.Generic;
using DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine.Serialization;

namespace DialogueSystem.Editor.Nodes
{
    [Serializable]
    public class ChoiceNode : Node
    {
        public string choiceAsset = "choiceAsset";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<GameChoice>(choiceAsset).WithDisplayName("Choice").Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort("Input").Build();

            if (GetNodeOptionByName(choiceAsset).TryGetValue(out GameChoice choice))
            {
                if (choice == null)
                    return;
                for (int i = 0; i < choice.Choices.Count; i++)
                {
                    context.AddOutputPort($"Choice{i}").WithDisplayName($"{choice.Choices[i]}").Build();
                }
            }
        }
    }
}
