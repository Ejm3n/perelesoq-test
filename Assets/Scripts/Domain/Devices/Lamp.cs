using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class Lamp : IDevice, IConsumable, ISwitchable, IInputAccepting, IElectricNode
    {
        private IElectricNode _input;
        public DeviceId Id { get; private set; }
        public string Name => "Lamp";
        public event Action<bool> OnSwitch;
        public bool IsOn { get; private set; }
        public float RatedPower { get; }
        public float ConsumedEnergy { get; private set; }

        public Lamp(IElectricNode input, DeviceId id, float energyRequired)
        {
            _input = input;
            Id = id;
            RatedPower = energyRequired;
        }

        public void Tick(float delta)
        {
            if (IsOn)
                ConsumedEnergy += RatedPower / 3600f * delta; // потому что в час а не в секунду.
        }

        public void ConnectInput(IElectricNode node)
        {
            _input = node;
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

        public void Switch(bool state)
        {
            IsOn = state;
            RefreshState();
        }

        public bool HasCurrent => IsOn && _input.HasCurrent;
    }
}