using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public class CameraSceneView : SceneViewBase<CameraDevice>
    {
        [SerializeField] private Transform cameraTransform; // дочерний Camera объект
        [SerializeField] private Vector3 cameraRotation;

        protected override void OnDeviceBound(CameraDevice cam)
        {
            _device = cam;

            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;

            cam.OnSelected += ApplyCameraPosition;

            if (cam.IsSelected)
                ApplyCameraPosition(true);
        }

        private void ApplyCameraPosition(bool isSelected)
        {
            if (!isSelected || cameraTransform == null) return;
            cameraTransform.SetParent(transform, false);
            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localEulerAngles = cameraRotation;
        }
    }
}