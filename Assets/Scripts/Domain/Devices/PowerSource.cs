using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class PowerSource : IDevice, IElectricNode
    {
        public DeviceId Id { get; private set; }
        public string Name => "Power Source";

        public float CurrentPower { get; private set; }
        public float TotalConsumedEnergy { get; private set; }
        public float Time { get; private set; }
        public event Action<float, float> OnPowerChange; // current power, total consumed energy
        public event Action<float> OnTimeChange; // time

        private readonly List<IConsumable> _consumers = new();

        public void RegisterConsumer(IConsumable consumer) => _consumers.Add(consumer);

        public PowerSource(DeviceId id)
        {
            Id = id;
        }

        public void Tick(float deltaTime)
        {
            CurrentPower = 0f;
            foreach (var c in _consumers)
            {
                if (c.IsOn)
                {
                    CurrentPower += c.RatedPower;
                    TotalConsumedEnergy += c.RatedPower * deltaTime / 1000f; // W*s -> kWh
                }
            }

            OnPowerChange?.Invoke(CurrentPower, TotalConsumedEnergy);

            Time += deltaTime;
            OnTimeChange?.Invoke(Time);
        }

        public void ConnectInput(IElectricNode input) { }
        public void AddOutput(IElectricNode output) { }
        public bool HasCurrent => true; // Always powered
    }
}
