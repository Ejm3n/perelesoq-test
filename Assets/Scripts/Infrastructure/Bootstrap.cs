using UnityEngine;
using SmartHome.Domain;
using SmartHome.Application;
using SmartHome.Presentation;
using SmartHome.Serialization;
using System.Collections.Generic;

namespace SmartHome.Infrastructure
{
    /// <summary>
    /// MonoBehaviour-инициализатор приложения.
    /// Привязывает ассет, репозиторий, фабрику и запускает SimulationLoop.
    /// </summary>
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private WidgetFactory _widgetFactory;
        [SerializeField] private PowerSourceView _powerSourceView;
        [SerializeField] private ElectricNetworkAsset _electricAsset;
        private DeviceRepository _repo;

        /// <summary>
        /// Главная точка запуска симуляции.
        /// Загружает граф из ассета, инициализирует use cases, UI и симуляцию.
        /// </summary>
        private void Start()
        {
            // 1. Подготовка репозитория и списка камер
            _repo = new DeviceRepository();
            var cameras = new List<CameraDevice>();

            // 2. Загрузка графа и наполнение
            Dictionary<string, IElectricNode> nodeMap = ElectricNetworkBuilder.BuildFromAsset(_electricAsset, _repo, cameras);

            // 3. Сбор имён
            foreach (var def in _electricAsset.devices)
            {
                var id = new DeviceId(def.id);
            }

            // 4. Инициализация UseCases
            var toggleUC = new ToggleDeviceUseCase(_repo);
            var selectCameraUC = new SelectCameraUseCase(cameras);
            selectCameraUC.Select(cameras[0]);

            // 5. UI: PowerSource
            PowerSource powerSource = null;
            foreach (var node in nodeMap.Values)
            {
                if (node is PowerSource power)
                {
                    _powerSourceView.Init(power);
                    powerSource = power;
                    break;
                }
            }

            // 6. UI: виджеты
            _widgetFactory.Init(_repo, toggleUC, selectCameraUC, powerSource);

            // 7. Симуляция
            gameObject.AddComponent<SimulationLoop>().Init(_repo);
        }
    }
}
