using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Presentation
{
    /// <summary>
    /// Отображает визуальное состояние логического гейта.
    /// </summary>
    public class GateSceneView : SwitchableMaterialSceneViewBase<LogicGate>
    {
        [SerializeField] private Renderer _inputA;
        [SerializeField] private Renderer _inputB;

        private void Update()
        {
            if (_device == null) return;

            if (_inputA && _device.A != null)
                _inputA.material = _device.A.HasCurrent ? _activeMat : _inactiveMat;

            if (_inputB && _device.B != null)
                _inputB.material = _device.B.HasCurrent ? _activeMat : _inactiveMat;
        }
    }
}
