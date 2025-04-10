using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class ElectricSwitch : IDevice, IElectricNode, ISwitchable
    {
        private readonly IElectricNode _input;
        public DeviceId Id { get; } = DeviceId.NewId();
        public string Name => "Switch";
        public bool IsOn { get; private set; }
        public bool HasCurrent => IsOn && _input.HasCurrent;
        public float CurrentPower => 0f;
        public event Action<bool> OnSwitch;
        public ElectricSwitch(IElectricNode input) => _input = input;
        public void Switch(bool state) => IsOn = state;
        public void Tick(float _) { }
    }
}