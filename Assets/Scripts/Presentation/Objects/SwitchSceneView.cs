using UnityEngine;
using SmartHome.Domain;
using SmartHome.Application;

namespace SmartHome.Presentation
{
    public class SwitchSceneView : SwitchableMaterialSceneViewBase<ElectricSwitch>
    {
        private ToggleDeviceUseCase _useCase;

        public void BindUseCase(ToggleDeviceUseCase useCase)
        {
            _useCase = useCase;
        }

        public void OnClick()
        {
            if (_useCase == null || _device == null) return;
            var newState = !_device.IsOn;
            _useCase.Execute(_device.Id, newState);
        }
    }
}
