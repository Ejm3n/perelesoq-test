using System;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class CleanerBot : IDevice, IConsumable, IInputAccepting, IElectricNode
    {
        private IElectricNode _input;

        public DeviceId Id { get; }
        public bool IsOn { get; private set; }

        public float BatteryCapacity { get; } = 60f;
        public float BatteryLevel { get; private set; } = 60f;
        public float DrainRate { get; } = 1.0f; // разрядка
        public float ChargeRate { get; } = 2.0f; // скорость зарядки

        public float RatedPower => IsOn ? DrainRate : 0f;
        public float ConsumedEnergy { get; private set; }

        public bool HasCurrent => BatteryLevel > 0f;

        public event Action<bool> OnSwitch;

        public CleanerBot(DeviceId id)
        {
            Id = id;
        }

        public void Switch(bool state)
        {
            IsOn = state;
            OnSwitch?.Invoke(state);
        }

        public void Tick(float deltaTime)
        {
            if (IsOn)
            {
                BatteryLevel -= DrainRate * deltaTime;
                BatteryLevel = Mathf.Max(BatteryLevel, 0f);
                ConsumedEnergy += DrainRate * deltaTime;
            }

            if (_input?.HasCurrent == true && BatteryLevel < BatteryCapacity)
            {
                BatteryLevel += ChargeRate * deltaTime;
                BatteryLevel = Mathf.Min(BatteryLevel, BatteryCapacity);
            }
        }

        public void ConnectInput(IElectricNode input) => _input = input;

        public void RefreshState() { } // необязательно
    }
}
