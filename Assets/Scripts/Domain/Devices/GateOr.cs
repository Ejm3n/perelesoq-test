using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class GateOr : IDevice, IElectricNode
    {
        private readonly IElectricNode _a;
        private readonly IElectricNode _b;
        public DeviceId Id { get; } = DeviceId.NewId();
        public string Name => "Gate OR";
        public bool HasCurrent => _a.HasCurrent || _b.HasCurrent;
        public GateOr(IElectricNode a, IElectricNode b) { _a = a; _b = b; }
        public void Tick(float _) { }
    }
}
