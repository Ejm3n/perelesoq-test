using UnityEngine;
using SmartHome.Domain;
using SmartHome.Presentation;

namespace SmartHome.Presentation
{
    /// <summary>
    /// свет в настольной лампе(торшере) является настоящим Light, причем их два, один вверх светит, другой вниз. логика не как у всех, пришлось новый класс написать.
    /// получилось довольно универсально, в будущем можно будет отделить логику в абстрактный класс.
    /// </summary>
    public class TableLampSceneView : SceneViewBase<Lamp>
    {
        [SerializeField] private GameObject[] _targets;

        protected override void OnDeviceBound(Lamp lamp)
        {
            _device = lamp;
            lamp.OnSwitch += UpdateState;
            UpdateState(lamp.IsOn);
        }

        private void UpdateState(bool isOn)
        {
            foreach (var obj in _targets)
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
