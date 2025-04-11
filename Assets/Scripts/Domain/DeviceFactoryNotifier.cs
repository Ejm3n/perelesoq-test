using System;
using SmartHome.Domain;

namespace SmartHome.Domain
{
    public static class DeviceFactoryNotifier
    {
        public static event Action<DeviceId, IDevice> OnDeviceCreated;

        public static void Notify(DeviceId id, IDevice device)
        {
            OnDeviceCreated?.Invoke(id, device);
        }
    }
}