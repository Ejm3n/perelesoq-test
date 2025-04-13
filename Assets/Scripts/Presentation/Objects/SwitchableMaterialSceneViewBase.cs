using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public abstract class SwitchableMaterialSceneViewBase<T> : SceneViewBase<T> where T : class, ISwitchable, IDevice
    {
        [SerializeField] protected Renderer _targetRenderer;
        [SerializeField] protected Material _activeMat;
        [SerializeField] protected Material _inactiveMat;

        protected override void OnDeviceBound(T device)
        {
            device.OnSwitch += UpdateVisual;
            UpdateVisual(device.IsOn);
        }

        /// <summary>
        /// Меняет материал рендера в зависимости от состояния устройства.
        /// </summary>
        protected virtual void UpdateVisual(bool isOn)
        {
            if (_targetRenderer != null)
                _targetRenderer.material = isOn ? _activeMat : _inactiveMat;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_device != null)
                _device.OnSwitch -= UpdateVisual;
        }
    }
}
