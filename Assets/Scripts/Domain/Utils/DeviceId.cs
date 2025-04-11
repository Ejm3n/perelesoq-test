using System;

namespace SmartHome.Domain
{
    [Serializable]
    public struct DeviceId : IEquatable<DeviceId>
    {
        public string Value;

        public DeviceId(string value)
        {
            Value = value;
        }

        public static DeviceId NewId() => new DeviceId(Guid.NewGuid().ToString());

        public override string ToString() => Value;

        public bool Equals(DeviceId other) => Value == other.Value;

        public override bool Equals(object obj) => obj is DeviceId other && Equals(other);

        public override int GetHashCode() => Value != null ? Value.GetHashCode() : 0;

        public static bool operator ==(DeviceId a, DeviceId b) => a.Equals(b);
        public static bool operator !=(DeviceId a, DeviceId b) => !a.Equals(b);
    }
}
