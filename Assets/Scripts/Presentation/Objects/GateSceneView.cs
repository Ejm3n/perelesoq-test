using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Presentation
{
    public class GateSceneView : SwitchableMaterialSceneViewBase<LogicGate>
    {
        [SerializeField] private Renderer _inputA;
        [SerializeField] private Renderer _inputB;

        private void Update()
        {
            if (_device == null) return;

            if (_inputA && _device.A != null)
                _inputA.material = _device.A.HasCurrent ? activeMat : inactiveMat;

            if (_inputB && _device.B != null)
                _inputB.material = _device.B.HasCurrent ? activeMat : inactiveMat;
        }
    }
}
