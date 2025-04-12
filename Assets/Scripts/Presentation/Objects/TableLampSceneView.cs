using UnityEngine;
using SmartHome.Domain;
using SmartHome.Presentation;

namespace SmartHome.Presentation
{
    public class TableLampSceneView : SceneViewBase<Lamp>
    {
        [SerializeField] private GameObject[] targets;

        protected override void OnDeviceBound(Lamp lamp)
        {
            _device = lamp;
            lamp.OnSwitch += UpdateState;
            UpdateState(lamp.IsOn);
        }

        private void UpdateState(bool isOn)
        {
            foreach (var obj in targets)
            {
                if (obj != null)
                    obj.SetActive(isOn);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_device != null)
                _device.OnSwitch -= UpdateState;
        }
    }
}
