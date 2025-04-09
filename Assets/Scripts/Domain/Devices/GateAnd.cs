using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class GateAnd : LogicGate
    {
        public GateAnd(IElectricNode a, IElectricNode b) : base(a, b) { }
        public override bool HasCurrent => _a.HasCurrent && _b.HasCurrent;
    }
}
