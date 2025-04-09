using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Application
{
    public sealed class DeviceRepository : IDeviceRepository
    {
        private readonly Dictionary<DeviceId, IDevice> _map = new();
        public IEnumerable<IDevice> All => _map.Values;
        public void Add(IDevice device) => _map[device.Id] = device;
        public T Get<T>(DeviceId id) where T : class, IDevice => _map[id] as T;
    }
}
