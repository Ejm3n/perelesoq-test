using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public abstract class SceneViewBase<T> : MonoBehaviour where T : class, IDevice
    {
        [SerializeField] protected string id;
        protected T _device;

        protected virtual void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        protected virtual void TryBind(DeviceId deviceId, IDevice device)
        {
            if (deviceId.Value != id) return;
            if (device is not T typed) return;

            _device = typed;
            OnDeviceBound(_device);
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }

        protected abstract void OnDeviceBound(T device);

        protected virtual void OnDestroy()
        {
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }
    }
}
