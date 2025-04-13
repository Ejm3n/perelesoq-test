using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    /// <summary>
    /// Обновляет своё состояние и пробрасывает его на выходы.
    /// </summary>
    public sealed class ElectricSwitch : IDevice, IElectricNode, ISwitchable, IInputAccepting, IOutputAccepting
    {
        public DeviceId Id { get; private set; }
        public bool IsOn { get; private set; }
        public bool HasCurrent => IsOn && _input?.HasCurrent == true;
        public event Action<bool> OnSwitch;
        private IElectricNode _input;
        private readonly List<IElectricNode> _outputs = new();

        public ElectricSwitch(IElectricNode input, DeviceId id)
        {
            _input = input;
            Id = id;
        }

        public void Switch(bool state)
        {
            IsOn = state;
            OnSwitch?.Invoke(HasCurrent);
            RefreshOutputs();
        }

        public void RefreshOutputs()
        {
            foreach (var output in _outputs)
            {
                if (output is ISwitchable sw)
                {
                    sw.RefreshState();
                }
            }
        }

        public void ConnectOutput(IElectricNode output) => _outputs.Add(output);

        public void RefreshState()
        {
            OnSwitch?.Invoke(HasCurrent);
            RefreshOutputs();
        }


        public void Tick(float _) { }

        public void SetInput(IElectricNode input) => _input = input;

        public void ConnectInput(IElectricNode input) => _input = input;

        public void AddOutput(IElectricNode output) => _outputs.Add(output);
    }

}