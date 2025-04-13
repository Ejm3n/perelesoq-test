using UnityEngine;
using SmartHome.Domain;
using TMPro;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

namespace SmartHome.Presentation
{
    /// <summary>
    /// Отображает состояние двери (цвет, статус) и анимирует открытие/закрытие.
    /// </summary>
    public class DoorSceneView : SceneViewBase<DoorDrive>
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Vector3 _closedRotation;
        [SerializeField] private Vector3 _openRotation;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private Color _openColor = Color.green;
        [SerializeField] private Color _closedColor = Color.red;
        [SerializeField] private Color _movingColor = Color.yellow;
        private DoorDrive _door;

        void Update()
        {
            if (_door == null) return;
            var t = _door.Progress;
            _targetTransform.localRotation = Quaternion.Euler(Vector3.Lerp(_closedRotation, _openRotation, t));
        }

        protected override void OnDeviceBound(DoorDrive door)
        {
            _door = door;
            door.OnSwitch += UpdateStatusText;
            UpdateStatusText(door.IsOpen);
        }

        /// <summary>
        /// Обновляет текст и цвет статуса в зависимости от состояния двери.
        /// </summary>
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
