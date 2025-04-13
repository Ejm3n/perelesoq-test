using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    /// <summary>
    /// Базовый интерфейс для всех устройств.
    /// </summary>
    public interface IDevice
    {
        DeviceId Id { get; }
        void Tick(float deltaTime);
    }
}
