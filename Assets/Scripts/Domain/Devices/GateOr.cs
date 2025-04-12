namespace SmartHome.Domain
{
    public sealed class GateOr : LogicGate
    {
        public GateOr(DeviceId id) : base(id) { }
        public override bool HasCurrent => A?.HasCurrent == true || B?.HasCurrent == true;
    }
}
