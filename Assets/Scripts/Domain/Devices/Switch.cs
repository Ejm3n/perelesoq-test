using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class Switch : IDevice, IElectricNode
    {
        private readonly IElectricNode _input;
        public DeviceId Id { get; } = DeviceId.NewId();
        public string Name => "Switch";

        public bool IsOn { get; private set; }
        public bool HasCurrent => IsOn && _input.HasCurrent;

        public Switch(IElectricNode input) => _input = input;

        public void Toggle() => IsOn = !IsOn;
        public void SwitchState(bool state) => IsOn = state;
        public void Tick(float _) { /* no-op */ }
    }
}