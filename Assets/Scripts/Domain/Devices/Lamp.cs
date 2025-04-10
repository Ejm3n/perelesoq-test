using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class Lamp : IDevice, IConsumable, ISwitchable
    {
        private readonly IElectricNode _input;
        public DeviceId Id { get; } = DeviceId.NewId();
        public string Name => "Lamp";
        public event Action<bool> OnSwitch;
        public bool IsOn { get; private set; }
        public float RatedPower { get; }
        public float ConsumedEnergy { get; private set; }

        public Lamp(IElectricNode input, float ratedPower = 100f)
        {
            _input = input;
            RatedPower = ratedPower;
        }

        public void Switch(bool state)
        {
            IsOn = state && _input.HasCurrent;
            OnSwitch?.Invoke(state);
        }
        public void Tick(float deltaTime)
        {
            if (IsOn)
                ConsumedEnergy += RatedPower * deltaTime / 1000f;
        }
    }
}