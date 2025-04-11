using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public class CameraSceneView : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private Transform cameraTransform; // дочерний Camera объект
        [SerializeField] private Vector3 cameraRotation;

        private CameraDevice _device;

        private void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        private void TryBind(DeviceId deviceId, IDevice device)
        {
            if (deviceId.Value != id || device is not CameraDevice cam) return;
            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;
            _device = cam;
            _device.OnSelected += ApplyCameraPosition;

            // если камера выбрана с самого начала
            if (cam.IsSelected)
                ApplyCameraPosition(true);

            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }

        private void ApplyCameraPosition(bool isSelected)
        {
            if (!isSelected || cameraTransform == null) return;

            Debug.Log("ApplyCameraPosition");
            cameraTransform.SetParent(transform, false);
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localEulerAngles = cameraRotation;
        }

        private void OnDestroy()
        {
            if (_device != null)
                _device.OnSelected -= ApplyCameraPosition;

            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }
    }
}