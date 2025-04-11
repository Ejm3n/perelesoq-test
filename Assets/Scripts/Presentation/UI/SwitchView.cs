using System.Collections;
using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace SmartHome.Presentation
{
    public sealed class SwitchView : DeviceWidgetView
    {
        [SerializeField] private Toggle _toggle;
        private Domain.ElectricSwitch _switch;
        private ToggleDeviceUseCase _useCase;
        private DeviceId _id;

        public void Init(ElectricSwitch sw, ToggleDeviceUseCase uc, string name)
        {
            _switch = sw;
            _useCase = uc;
            _id = (sw as IDevice)!.Id;
            SetName(name);
            _toggle.onValueChanged.AddListener(OnClick);
            OnClick(_toggle.isOn);
        }
        private void OnClick(bool isOn)
        {
            _useCase.Execute(_id, isOn);
            SetStatus(isOn ? "ON" : "OFF");
        }
    }
}
