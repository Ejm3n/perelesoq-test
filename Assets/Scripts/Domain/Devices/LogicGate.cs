using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public abstract class LogicGate : IDevice, IElectricNode
    {
        protected readonly IElectricNode _a;
        protected readonly IElectricNode _b;
        public DeviceId Id { get; } = new DeviceId(); // Changed from DeviceId.New() to new DeviceId()
        public string Name => GetType().Name;
        public float CurrentPower => 0f;

        protected LogicGate(IElectricNode a, IElectricNode b)
        {
            _a = a; _b = b;
        }
        public abstract bool HasCurrent { get; }
        public void Tick(float _) { }
        public void ConnectInput(IElectricNode input) { }
        public void AddOutput(IElectricNode output) { }
    }
}
