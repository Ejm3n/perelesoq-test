using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public abstract class SwitchableMaterialSceneViewBase<T> : SceneViewBase<T> where T : class, ISwitchable, IDevice
    {
        [SerializeField] protected Renderer targetRenderer;
        [SerializeField] protected Material activeMat;
        [SerializeField] protected Material inactiveMat;

        protected override void OnDeviceBound(T device)
        {
            device.OnSwitch += UpdateVisual;
            UpdateVisual(device.IsOn);
        }

        protected virtual void UpdateVisual(bool isOn)
        {
            if (targetRenderer != null)
                targetRenderer.material = isOn ? activeMat : inactiveMat;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_device != null)
                _device.OnSwitch -= UpdateVisual;
        }
    }
}
