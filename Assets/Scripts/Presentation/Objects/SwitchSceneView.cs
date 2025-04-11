using UnityEngine;
using SmartHome.Domain;
using SmartHome.Application;

namespace SmartHome.Presentation
{
    public class SwitchSceneView : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private Renderer visualRenderer;
        [SerializeField] private Material onMaterial;
        [SerializeField] private Material offMaterial;

        private ElectricSwitch _switch;
        private ToggleDeviceUseCase _useCase;
        private bool _isOn;

        private void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        private void TryBind(DeviceId deviceId, IDevice device)
        {
            if (deviceId.Value != id || device is not ElectricSwitch sw) return;

            _switch = sw;
            _isOn = _switch.IsOn;

            _switch.OnSwitch += UpdateVisual;
            UpdateVisual(_switch.HasCurrent);

            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }

        private void UpdateVisual(bool hasCurrent)
        {
            _isOn = hasCurrent;
            if (visualRenderer != null)
                visualRenderer.material = _isOn ? onMaterial : offMaterial;
        }

        public void BindUseCase(ToggleDeviceUseCase useCase)
        {
            _useCase = useCase;
        }

        public void OnClick()
        {
            if (_useCase == null || _switch == null) return;
            var newState = !_switch.IsOn;
            _useCase.Execute(_switch.Id, newState);
        }

        private void OnDestroy()
        {
            if (_switch != null)
                _switch.OnSwitch -= UpdateVisual;

            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }
    }
}
