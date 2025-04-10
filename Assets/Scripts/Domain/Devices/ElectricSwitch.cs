using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class ElectricSwitch : IDevice, IElectricNode, ISwitchable, IInputAccepting
    {
        private IElectricNode _input;
        private readonly List<IElectricNode> _outputs = new();

        public DeviceId Id { get; } = DeviceId.NewId();
        public string Name => "Switch";
        public bool IsOn { get; private set; }
        public bool HasCurrent => IsOn && _input?.HasCurrent == true;

        public event Action<bool> OnSwitch;

        public ElectricSwitch(IElectricNode input) => _input = input;

        public void Switch(bool state)
        {
            IsOn = state;
            foreach (var output in _outputs)
            {
                // можно уведомить их или вызвать Tick/Update
            }

            OnSwitch?.Invoke(HasCurrent);
        }

        public void Tick(float _) { }

        public void SetInput(IElectricNode input) => _input = input;

        public void ConnectInput(IElectricNode input) => _input = input;

        public void AddOutput(IElectricNode output) => _outputs.Add(output);
    }

}