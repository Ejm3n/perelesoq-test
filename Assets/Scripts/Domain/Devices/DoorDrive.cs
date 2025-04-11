using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class DoorDrive : IDevice, IConsumable, IInputAccepting, IElectricNode
    {
        private IElectricNode _input;
        public DeviceId Id { get; private set; }
        public string Name => "Door Drive";
        public event Action<bool> OnSwitch;
        public bool IsOn => _progress > 0f && _progress < 1f;
        public float RatedPower { get; private set; }
        public float ConsumedEnergy { get; private set; }
        private float _progress; // 0 closed, 1 open
        private bool _targetOpen;

        public DoorDrive(IElectricNode input, DeviceId id, float energyRequired)
        {
            _input = input;
            Id = id;
            RatedPower = energyRequired;
        }

        public void Switch(bool open) => _targetOpen = open;

        public void Tick(float deltaTime)
        {
            if (!_input.HasCurrent) return;
            if (_progress == (_targetOpen ? 1f : 0f)) return;

            var dir = _targetOpen ? 1f : -1f;
            _progress = Mathf.Clamp01(_progress + dir * deltaTime / 5f); // 5 seconds
            ConsumedEnergy += RatedPower * deltaTime;
        }
        public void RefreshState()
        {
            // var prev = IsOn;
            //IsOn = IsOn && _input.HasCurrent;
            //if (IsOn != prev)
            //    OnSwitch?.Invoke(IsOn);
        }

        public void ConnectInput(IElectricNode input)
        {
            _input = input;
        }

        public bool HasCurrent => _input.HasCurrent;
    }

}