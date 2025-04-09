using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class GateOr : LogicGate
    {
        public GateOr(IElectricNode a, IElectricNode b) : base(a, b) { }
        public override bool HasCurrent => _a.HasCurrent || _b.HasCurrent;
    }
}
