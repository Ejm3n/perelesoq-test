namespace SmartHome.Domain
{
    /// <summary>
    /// OR логический гейт — выдаёт ток если хотя бы один вход активен.
    /// </summary>
    public sealed class GateOr : LogicGate
    {
        public GateOr(DeviceId id) : base(id) { }
        public override bool HasCurrent => A?.HasCurrent == true || B?.HasCurrent == true;
    }
}
