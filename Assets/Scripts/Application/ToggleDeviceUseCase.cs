using SmartHome.Domain;

namespace SmartHome.Application
{
    public sealed class ToggleDeviceUseCase
    {
        private readonly IDeviceRepository _repo;
        public ToggleDeviceUseCase(IDeviceRepository repo) => _repo = repo;

        public void ExecuteRepository(bool isOn)
        {
            foreach (var id in _repo.All)
            {
                Execute(id.Id, isOn);
            }
        }

        public void Execute(DeviceId id, bool isOn)
        {
            if (_repo.Get<IDevice>(id) is ISwitchable s)
            {
                s.Switch(isOn);
            }
        }
    }
}
