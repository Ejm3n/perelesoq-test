using UnityEngine;

namespace SmartHome.Domain
{
    public sealed class ChargingStation : IDevice, IElectricNode, IInputAccepting, IOutputAccepting
    {
        private IElectricNode _input;
        private IElectricNode _output;
        public DeviceId Id { get; }

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
    }
}
