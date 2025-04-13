namespace SmartHome.Domain
{
    /// <summary>
    /// AND логический гейт — выдаёт ток только если оба входа активны.
    /// </summary>
    public sealed class GateAnd : LogicGate
    {
        public GateAnd(DeviceId id) : base(id) { }
        public override bool HasCurrent => A?.HasCurrent == true && B?.HasCurrent == true;
    }
}
