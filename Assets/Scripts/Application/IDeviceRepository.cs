using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Application
{
    /// <summary>
    /// Репозиторий всех устройств системы, обеспечивает доступ по ID.
    /// </summary>
    public interface IDeviceRepository
    {
        IEnumerable<IDevice> All { get; }
        T Get<T>(DeviceId id) where T : class, IDevice;
        void Add(IDevice device);
    }
}
