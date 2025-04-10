using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Presentation
{
    public sealed class WidgetFactory : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private LampView _lampPrefab;
        [SerializeField] private SwitchView _switchPrefab;
        [SerializeField] private DoorDriveView _doorPrefab;
        [SerializeField] private CameraView _cameraPrefab;

        private ToggleDeviceUseCase _toggleUC;
        private SelectCameraUseCase _cameraUC;
        private PowerSource _powerSource;

        public void Init(IDeviceRepository repo, ToggleDeviceUseCase toggleUC, SelectCameraUseCase cameraUC, PowerSource powerSource, Dictionary<DeviceId, string> names)
        {
            _toggleUC = toggleUC;
            _cameraUC = cameraUC;
            _powerSource = powerSource;
            DeviceFactoryNotifier.Notify(powerSource.Id, powerSource);
            foreach (var device in repo.All)
            {
                if (device is IConsumable consumable)
                    _powerSource.RegisterConsumer(consumable);
                if (device is Lamp lamp)
                {
                    var view = Instantiate(_lampPrefab, _root);
                    view.Init(lamp, names[lamp.Id]);
                    DeviceFactoryNotifier.Notify(lamp.Id, lamp);
                }
                else if (device is ElectricSwitch sw)
                {
                    var view = Instantiate(_switchPrefab, _root);
                    view.Init(sw, _toggleUC, names[sw.Id]);
                    DeviceFactoryNotifier.Notify(sw.Id, sw);
                }
                else if (device is DoorDrive door)
                {
                    var view = Instantiate(_doorPrefab, _root);
                    view.Init(door, names[door.Id]);
                    DeviceFactoryNotifier.Notify(door.Id, door);
                }
                else if (device is CameraDevice camera)
                {
                    var view = Instantiate(_cameraPrefab, _root);
                    view.Init(camera, _cameraUC);
                    DeviceFactoryNotifier.Notify(camera.Id, camera);
                }
                // etc... можно вынести в словарь-реестр фабрик, если устройств будет много
            }
        }
    }
}
