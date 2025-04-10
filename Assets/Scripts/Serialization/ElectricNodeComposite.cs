using System;
using System.Collections.Generic;
using SmartHome.Domain;

public sealed class ElectricNodeComposite : IElectricNode
{
    private readonly List<IElectricNode> _inputs = new();
    private readonly List<IElectricNode> _outputs = new();
    private readonly Func<IEnumerable<IElectricNode>, bool> _logic;

    public ElectricNodeComposite(Func<IEnumerable<IElectricNode>, bool> logic)
    {
        _logic = logic;
    }

    public void AddInput(IElectricNode node)
    {
        _inputs.Add(node);
    }

    public bool HasCurrent => _logic(_inputs);

    public void ConnectInput(IElectricNode input)
    {
        _inputs.Add(input);
    }

    public void AddOutput(IElectricNode output)
    {
        _outputs.Add(output);
    }
}
