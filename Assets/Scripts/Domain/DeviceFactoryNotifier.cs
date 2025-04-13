using System;
using SmartHome.Domain;

namespace SmartHome.Domain
{
    /// <summary>
    /// Централизованная точка уведомлений о создании устройств в рантайме.
    /// Используется WidgetFactory/SceneView для биндинга.
    /// </summary>
    public static class DeviceFactoryNotifier
    {
        public static event Action<DeviceId, IDevice> OnDeviceCreated;

        public static void Notify(DeviceId id, IDevice device)
        {
            OnDeviceCreated?.Invoke(id, device);
        }
    }
}