using UnityEngine;
using SmartHome.Domain;
using TMPro;

namespace SmartHome.Presentation
{
    public class DoorSceneView : SceneViewBase<DoorDrive>
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Vector3 closedRotation;
        [SerializeField] private Vector3 openRotation;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private Color _openColor = Color.green;
        [SerializeField] private Color _closedColor = Color.red;
        [SerializeField] private Color _movingColor = Color.yellow;

        private DoorDrive _door;

        protected override void OnDeviceBound(DoorDrive door)
        {
            _door = door;
            door.OnSwitch += UpdateStatusText;
            UpdateStatusText(door.IsOpen);
        }

        void Update()
        {
            if (_door == null) return;
            var t = _door.Progress;
            targetTransform.localRotation = Quaternion.Euler(Vector3.Lerp(closedRotation, openRotation, t));
        }

        private void UpdateStatusText(bool isOpen)
        {
            if (_statusText == null) return;
            if (_door.IsMoving)
            {
                _statusText.text = "MOVING";
                _statusText.color = _movingColor;
            }
            else if (_door.IsOpen)
            {
                _statusText.text = "OPEN";
                _statusText.color = _openColor;
            }
            else
            {
                _statusText.text = "CLOSED";
                _statusText.color = _closedColor;
            }
        }
    }
}
