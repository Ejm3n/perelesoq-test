using UnityEngine;
using SmartHome.Domain;
using TMPro;

namespace SmartHome.Presentation
{
    public class GateSceneView : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private Renderer _inputA;
        [SerializeField] private Renderer _inputB;
        [SerializeField] private Renderer _output;
        [SerializeField] private Material _activeMat;
        [SerializeField] private Material _inactiveMat;

        private LogicGate _gate;

        private void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        private void TryBind(DeviceId createdId, IDevice device)
        {
            if (createdId.Value != id) return;
            if (device is LogicGate gate)
            {
                _gate = gate;
                gate.OnSwitch += OnStateChanged;
                OnStateChanged(gate.IsOn);
                DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
                Debug.Log("GateSceneView: TryBind");
            }
        }

        private void Update()
        {
            if (_gate == null) return;

            if (_inputA && _gate.A != null)
                _inputA.material = _gate.A.HasCurrent ? _activeMat : _inactiveMat;

            if (_inputB && _gate.B != null)
                _inputB.material = _gate.B.HasCurrent ? _activeMat : _inactiveMat;
        }

        private void OnStateChanged(bool isOn)
        {
            _output.material = isOn ? _activeMat : _inactiveMat;
        }

        private void OnDestroy()
        {
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
            if (_gate != null)
                _gate.OnSwitch -= OnStateChanged;
        }
    }
}