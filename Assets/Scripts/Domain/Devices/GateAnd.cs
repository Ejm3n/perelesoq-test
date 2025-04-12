namespace SmartHome.Domain
{
    public sealed class GateAnd : LogicGate
    {
        public GateAnd(DeviceId id) : base(id) { }
        public override bool HasCurrent => A?.HasCurrent == true && B?.HasCurrent == true;
    }
}
