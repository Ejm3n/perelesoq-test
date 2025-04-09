using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class GateAnd : IDevice, IElectricNode
    {
        private readonly IElectricNode _a;
        private readonly IElectricNode _b;
        public DeviceId Id { get; } = DeviceId.NewId();
        public string Name => "Gate AND";
        public bool HasCurrent => _a.HasCurrent && _b.HasCurrent;
        public GateAnd(IElectricNode a, IElectricNode b) { _a = a; _b = b; }
        public void Tick(float _) { /* stateless */ }
    }
}
