using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public class DoorSceneView : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Vector3 closedRotation;
        [SerializeField] private Vector3 openRotation;

        private DoorDrive _door;

        void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        private void TryBind(DeviceId deviceId, IDevice device)
        {
            if (deviceId.Value != id) return;
            if (device is DoorDrive door)
            {
                _door = door;
                DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
            }
        }

        void Update()
        {
            if (_door == null) return;
            var t = _door.Progress;
            targetTransform.localRotation = Quaternion.Euler(Vector3.Lerp(closedRotation, openRotation, t));
        }

        void OnDestroy()
        {
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }
    }
}
