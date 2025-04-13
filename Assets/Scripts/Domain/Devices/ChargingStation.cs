using System;

namespace SmartHome.Domain
{
    /// <summary>
    /// Зарядная станция — источник тока для устройств с батареей (например, CleanerBot).
    /// Работает только если есть ток на входе.
    /// </summary>
    public sealed class ChargingStation : IDevice, IElectricNode, IInputAccepting, IOutputAccepting, ISwitchable
    {
        public DeviceId Id { get; }
        private IElectricNode _input;
        private IElectricNode _output;
        public bool IsOn { get; private set; }
        public event Action<bool> OnSwitch;

        public ChargingStation(DeviceId id)
        {
            Id = id;
        }

        public void Tick(float deltaTime)
        {
            // зарядка будет происходить в самом CleanerBot на основе .HasCurrent = true
        }

        public void ConnectOutput(IElectricNode output)
        {
            _output = output;
        }

        public bool HasCurrent => _input?.HasCurrent == true;

        public void ConnectInput(IElectricNode input)
        {
            _input = input;
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
    }
}
