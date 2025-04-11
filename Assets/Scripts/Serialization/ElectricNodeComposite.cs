using System;
using System.Collections.Generic;
using SmartHome.Domain;

public sealed class ElectricNodeComposite : IElectricNode
{
    public DeviceId Id { get; private set; }
    private readonly List<IElectricNode> _inputs = new();
    private readonly List<IElectricNode> _outputs = new();
    private readonly Func<IEnumerable<IElectricNode>, bool> _logic;

    public ElectricNodeComposite(Func<IEnumerable<IElectricNode>, bool> logic, DeviceId id)
    {
        _logic = logic;
        Id = id;
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
