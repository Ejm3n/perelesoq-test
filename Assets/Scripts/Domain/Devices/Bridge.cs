using System;
using System.Collections.Generic;


namespace SmartHome.Domain
{
    /// <summary>
    /// Устройство-переходник: пропускает ток, если ток есть на входе.
    /// Само не управляется — просто передаёт состояние дальше.
    /// </summary>
    public sealed class Bridge : IElectricNode, IDevice, IInputAccepting, IOutputAccepting, ISwitchable
    {
        public DeviceId Id { get; }
        public bool IsOn { get; private set; } // "включен", если по входу есть ток
        public bool HasCurrent => _input?.HasCurrent == true;
        public event Action<bool> OnSwitch;
        private IElectricNode _input;
        private readonly List<IElectricNode> _outputs = new();

        public Bridge(DeviceId id)
        {
            Id = id;
        }

        public void ConnectInput(IElectricNode input) => _input = input;
        public void ConnectOutput(IElectricNode output) => _outputs.Add(output);

        public void Switch(bool _) => RefreshState();

        /// <summary>
        /// Обновляет своё состояние в зависимости от входа, и обновляет все выходы.
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
    }

}