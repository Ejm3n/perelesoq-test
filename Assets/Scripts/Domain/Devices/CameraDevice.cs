using System;

namespace SmartHome.Domain
{
    /// <summary>
    /// Устройство-камера, умеет быть выбранной/невыбранной.
    /// </summary>
    public sealed class CameraDevice : IDevice
    {
        public DeviceId Id { get; }
        public event Action<bool> OnSelected;
        public bool IsSelected => _isSelected;
        private bool _isSelected;

        public CameraDevice(DeviceId id)
        {
            Id = id;
        }

        public void Select()
        {
            if (_isSelected) return;
            _isSelected = true;
            OnSelected?.Invoke(true);
        }

        public void Deselect()
        {
            if (!_isSelected) return;
            _isSelected = false;
            OnSelected?.Invoke(false);
        }

        public void Tick(float deltaTime) { }
    }
}
