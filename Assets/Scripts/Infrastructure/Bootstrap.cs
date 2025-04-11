using UnityEngine;
using SmartHome.Domain;
using SmartHome.Application;
using SmartHome.Presentation;
using SmartHome.Serialization;
using System.Collections.Generic;

namespace SmartHome.Infrastructure
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private WidgetFactory _widgetFactory;
        [SerializeField] private PowerSourceView _powerSourceView;
        [SerializeField] private ElectricNetworkAsset _electricAsset;

        private DeviceRepository _repo;

        private void Awake()
        {
            // 1. Загрузка графа из ассета
            Dictionary<string, IElectricNode> nodeMap = ElectricNetworkBuilder.BuildFromAsset(_electricAsset);

            // 2. Заполнение репозитория
            _repo = new DeviceRepository();
            foreach (var node in nodeMap.Values)
            {
                if (node is IDevice device)
                    _repo.Add(device);
            }

            // 3. Инициализация UseCases
            var toggleUC = new ToggleDeviceUseCase(_repo);
            var selectCameraUC = new SelectCameraUseCase(_repo);

            // 4. UI
            // Поиск PowerSource для инициализации UI — можно упростить, если у тебя их несколько
            foreach (var node in nodeMap.Values)
            {
                if (node is PowerSource power)
                {
                    _powerSourceView.Init(power);
                    break;
                }
            }

            _widgetFactory.Init(_repo, toggleUC, selectCameraUC);

            // 5. Симуляция
            gameObject.AddComponent<SimulationLoop>().Init(_repo);
        }
    }
}
