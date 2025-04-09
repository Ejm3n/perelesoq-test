using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class CameraDevice : IDevice
    {
        public DeviceId Id { get; } = DeviceId.NewId();
        public string Name => "Camera";
        public void Tick(float _) { }
    }
}