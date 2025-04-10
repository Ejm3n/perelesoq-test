using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public interface IDevice
    {
        DeviceId Id { get; }
        void Tick(float deltaTime);
    }
}
