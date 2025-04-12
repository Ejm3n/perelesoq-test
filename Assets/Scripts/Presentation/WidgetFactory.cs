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
        [SerializeField] private GateView _gatePrefab;
        [SerializeField] private CleanerBotView _cleanerBotPrefab;

        private ToggleDeviceUseCase _toggleUC;
        private SelectCameraUseCase _cameraUC;
        private PowerSource _powerSource;

        public void Init(IDeviceRepository repo, ToggleDeviceUseCase toggleUC, SelectCameraUseCase cameraUC, PowerSource powerSource)
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
                    view.Init(lamp);
                    DeviceFactoryNotifier.Notify(lamp.Id, lamp);
                }
                else if (device is ElectricSwitch sw)
                {
                    var view = Instantiate(_switchPrefab, _root);
                    view.Init(sw, _toggleUC);
                    DeviceFactoryNotifier.Notify(sw.Id, sw);
                }
                else if (device is DoorDrive door)
                {
                    var view = Instantiate(_doorPrefab, _root);
                    view.Init(door);
                    DeviceFactoryNotifier.Notify(door.Id, door);
                }
                else if (device is CameraDevice camera)
                {
                    var view = Instantiate(_cameraPrefab, _root);
                    view.Init(camera, _cameraUC);
                    DeviceFactoryNotifier.Notify(camera.Id, camera);
                }
                else if (device is LogicGate gate)
                {
                    var view = Instantiate(_gatePrefab, _root);
                    view.Init(gate);
                    DeviceFactoryNotifier.Notify(gate.Id, gate);
                }
                else if (device is CleanerBot bot)
                {
                    var view = Instantiate(_cleanerBotPrefab, _root);
                    view.Init(bot);
                    DeviceFactoryNotifier.Notify(bot.Id, bot);
                }
            }
        }
    }
}
