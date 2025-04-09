using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartHome.Domain;
using SmartHome.Application;

namespace SmartHome.Infrastructure
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Presentation.LampView _lampViewPrefab;
        [SerializeField] private Presentation.SwitchView _switchViewPrefab;
        [SerializeField] private Transform _uiRoot;

        private DeviceRepository _repo;
        private PowerSource _power;
        private Application.ToggleDeviceUseCase _toggleUC;

        private void Awake()
        {
            // 1. Create domain graph
            _power = new PowerSource();
            var wallSwitch = new ElectricSwitch(_power);
            var lamp = new Lamp(wallSwitch);

            // 2. Repo
            _repo = new DeviceRepository();
            _repo.Add(_power);
            _repo.Add(wallSwitch);
            _repo.Add(lamp);

            // 3. Use‑cases
            _toggleUC = new Application.ToggleDeviceUseCase(_repo);

            // 4. UI binding
            var lampView = Instantiate(_lampViewPrefab, _uiRoot);
            lampView.Init(lamp);

            var switchView = Instantiate(_switchViewPrefab, _uiRoot);
            switchView.Init(wallSwitch, _toggleUC);
        }
    }
}
