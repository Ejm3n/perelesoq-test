using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    /// <summary>
    /// Базовый класс для логических гейтов (AND/OR).
    /// Имеет 2 входа (A, B), список выходов и управляет цепочкой.
    /// </summary>
    public abstract class LogicGate : IDevice, IElectricNode, IInputAccepting, IOutputAccepting, ISwitchable
    {
        public IElectricNode A { get; private set; }
        public IElectricNode B { get; private set; }
        public DeviceId Id { get; }
        public bool IsOn { get; protected set; }
        public float CurrentPower => 0f;
        public abstract bool HasCurrent { get; }
        public event Action<bool> OnSwitch;
        protected readonly List<IElectricNode> _outputs = new();

        protected LogicGate(DeviceId id)
        {
            Id = id;
        }

        /// <summary>
        /// Подключает вход. Первый идёт в A, второй в B.
        /// </summary>
        public void ConnectInput(IElectricNode input)
        {
            if (A == null) A = input;
            else if (B == null) B = input;
            else Debug.LogWarning($"[LogicGate] More than two inputs connected to {Id}");
        }

        public void Switch(bool _) => RefreshState();

        /// <summary>
        /// Обновляет внутреннее состояние и оповещает выходы.
        /// </summary>
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
