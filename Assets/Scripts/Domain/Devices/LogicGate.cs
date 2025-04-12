using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public abstract class LogicGate : IDevice, IElectricNode, IInputAccepting, IOutputAccepting, ISwitchable
    {
        public IElectricNode A { get; private set; }
        public IElectricNode B { get; private set; }
        protected readonly List<IElectricNode> _outputs = new();

        public DeviceId Id { get; }
        public bool IsOn { get; protected set; }
        public float CurrentPower => 0f;
        public abstract bool HasCurrent { get; }

        public event Action<bool> OnSwitch;

        protected LogicGate(DeviceId id)
        {
            Id = id;
        }

        public void ConnectInput(IElectricNode input)
        {
            if (A == null) A = input;
            else if (B == null) B = input;
            else Debug.LogWarning($"[LogicGate] More than two inputs connected to {Id}");
        }

        public void AddOutput(IElectricNode output)
        {
            _outputs.Add(output);
        }

        public void Switch(bool _) => RefreshState();

        public void RefreshState()
        {
            var prev = IsOn;
            IsOn = HasCurrent;
            if (prev != IsOn)
                OnSwitch?.Invoke(IsOn);

            foreach (var o in _outputs)
                if (o is ISwitchable s) s.RefreshState();
        }

        public void Tick(float _) { }

        public void ConnectOutput(IElectricNode output)
        {
            _outputs.Add(output);
        }
    }
}
