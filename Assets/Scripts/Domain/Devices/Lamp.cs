using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class Lamp : IDevice, IConsumable, ISwitchable, IInputAccepting, IElectricNode
    {
        public DeviceId Id { get; private set; }
        public event Action<bool> OnSwitch;
        public bool IsOn { get; private set; }
        public float RatedPower { get { return _energyRequiredPerHour; } }
        public float ConsumedEnergy { get; private set; }
        private IElectricNode _input;
        private float _energyRequiredPerHour;

        public Lamp(IElectricNode input, DeviceId id, float energyRequired)
        {
            _input = input;
            Id = id;
            _energyRequiredPerHour = energyRequired;
            IsOn = true;
        }

        /// <summary>
        /// Учитывает потребление энергии при включённой лампе (Вт*ч).
        /// </summary>
        public void Tick(float delta)
        {
            if (IsOn)
                ConsumedEnergy += _energyRequiredPerHour / 3600f * delta; // потому что в час а не в секунду.
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

        public void SwitchState(bool state)
        {
            IsOn = state;
            RefreshState();
        }

        public bool HasCurrent => IsOn && _input.HasCurrent;
    }
}