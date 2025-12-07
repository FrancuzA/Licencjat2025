using Unity.GraphToolkit.Editor;
using UnityEngine;
using static Unity.GraphToolkit.Editor.Node;

public class MessageNode : Node
{
    public string actorField = "actorField";
    public string msgField = "Message";

    protected override void OnDefineOptions(IOptionDefinitionContext context)
    {
        context.AddOption<Actor>(actorField).WithDisplayName("Actor").Delayed();
        context.AddOption<string>(msgField).WithDisplayName("Message").Delayed();
    }

    protected override void OnDefinePorts(IPortDefinitionContext context)
    {
        context.AddInputPort("Input").Build();

        context.AddOutputPort("Output").Build();
    }
}
