using System.Collections;
using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace SmartHome.Presentation
{
    public sealed class SwitchView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private ISwitchable _switch;
        private ToggleDeviceUseCase _useCase;
        private DeviceId _id;

        public void Init(ISwitchable sw, ToggleDeviceUseCase uc)
        {
            _switch = sw;
            _useCase = uc;
            _id = (sw as IDevice)!.Id;
            _button.onClick.AddListener(OnClick);
        }
        private void OnClick() => _useCase.Execute(_id);
    }
}
