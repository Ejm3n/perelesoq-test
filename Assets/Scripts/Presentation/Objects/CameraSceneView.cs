using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public class CameraSceneView : SceneViewBase<CameraDevice>
    {
        [SerializeField] private Transform _cameraTransform; // дочерний Camera объект
        [SerializeField] private Vector3 _cameraRotation;

        protected override void OnDeviceBound(CameraDevice cam)
        {
            _device = cam;

            if (_cameraTransform == null)
                _cameraTransform = Camera.main.transform;

            cam.OnSelected += ApplyCameraPosition;

            if (cam.IsSelected)
                ApplyCameraPosition(true);
        }

        /// <summary>
        /// Переносит основную камеру в положение конкретного устройства.
        /// Используется при выборе камеры пользователем.
        /// </summary>
        private void ApplyCameraPosition(bool isSelected)
        {
            if (!isSelected || _cameraTransform == null) return;
            _cameraTransform.SetParent(transform, false);
            _cameraTransform.localPosition = Vector3.zero;
            _cameraTransform.localEulerAngles = _cameraRotation;
        }
    }
}