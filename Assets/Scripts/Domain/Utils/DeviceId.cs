using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public readonly struct DeviceId
    {
        public Guid Value { get; }

        public DeviceId(Guid value)
        {
            Value = value;
        }

        public static DeviceId NewId() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
