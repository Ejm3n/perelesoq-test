using System;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class DoorDrive : IDevice, IConsumable, IInputAccepting, IElectricNode, ISwitchable
    {
        private IElectricNode _input;
        public DeviceId Id { get; }
        public float RatedPower { get; }
        public float ConsumedEnergy { get; private set; }
        public event Action<bool> OnSwitch;
        public bool IsOn { get; private set; }
        public bool IsMoving => _isMoving;
        public bool IsOpen => _progress >= 1f;
        public bool IsClosed => _progress <= 0f;
        private float _progress; // 0 закрыто, 1 открыто
        private bool _targetOpen;
        private bool _isMoving;
        private float _useDuration;


        public DoorDrive(IElectricNode input, DeviceId id, float ratedPower, float useDuration)
        {
            _input = input;
            Id = id;
            RatedPower = ratedPower;
            _useDuration = useDuration;
        }

        /// <summary>
        /// Команда открыть/закрыть дверь.
        /// </summary>
        public void Switch(bool open)
        {
            if (_isMoving || _targetOpen == open) return;
            _targetOpen = open;
            _isMoving = true;
            OnSwitch?.Invoke(true);
        }

        /// <summary>
        /// Привод двери: открывается/закрывается со временем при наличии тока.
        /// </summary>
        public void Tick(float deltaTime)
        {
            if (!_input.HasCurrent || !_isMoving) return;

            var dir = _targetOpen ? 1f : -1f;
            _progress = Mathf.Clamp01(_progress + dir * deltaTime / _useDuration);

            float energyPerSecond = RatedPower / _useDuration;
            ConsumedEnergy += energyPerSecond * deltaTime;

            if (_progress == 0f || _progress == 1f)
            {
                _isMoving = false;
                OnSwitch?.Invoke(false);
            }
        }

        public void RefreshState()
        {
            var prev = IsOn;
            IsOn = _input.HasCurrent;
            if (IsOn != prev)
            {
                OnSwitch?.Invoke(IsOn);
            }
        }

        public void ConnectInput(IElectricNode input) => _input = input;
        public bool HasCurrent => _input.HasCurrent;
        public float Progress => _progress;
    }
}