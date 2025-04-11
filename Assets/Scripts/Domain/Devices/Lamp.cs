using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class Lamp : IDevice, IConsumable, ISwitchable, IInputAccepting, IElectricNode
    {
        private IElectricNode _input;
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


        public void Tick(float deltaTime)
        {
            if (IsOn)
                ConsumedEnergy += RatedPower * deltaTime / 1000f;
        }

        public void ConnectInput(IElectricNode node)
        {
            _input = node;
        }

        public void RefreshState()
        {
            var prev = IsOn;
            IsOn = _input.HasCurrent; // <-- без учета старого состояния
            if (IsOn != prev)
            {
                Debug.Log($"[Lamp] State changed → {IsOn}");
                OnSwitch?.Invoke(IsOn);
            }
        }

        public void Switch(bool state)
        {
            IsOn = state;
            Debug.Log($"[Lamp] Switch({state}) → calling RefreshState()");
            RefreshState();
        }

        public bool HasCurrent => IsOn && _input.HasCurrent;
    }
}