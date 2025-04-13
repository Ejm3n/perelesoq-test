using SmartHome.Domain;

namespace SmartHome.Application
{
    public sealed class ToggleDeviceUseCase
    {
        private readonly IDeviceRepository _repo;
        public ToggleDeviceUseCase(IDeviceRepository repo) => _repo = repo;

        public void Execute(DeviceId id, bool isOn)
        {
            if (_repo.Get<IDevice>(id) is ISwitchable s)
            {
                s.SwitchState(isOn);
            }
        }
    }
}
