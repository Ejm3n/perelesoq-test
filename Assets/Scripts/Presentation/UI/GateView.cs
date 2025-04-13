using UnityEngine;
using UnityEngine.UI;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public class GateView : DeviceWidgetView
    {
        /// <summary>
        /// Инициализирует GateView и привязывает его к логическому гейту.
        /// </summary>
        public void Init(LogicGate gate)
        {
            SetName(gate.Id.Value);
            UpdateUI(gate.IsOn);

            gate.OnSwitch += UpdateUI;
        }

        private void UpdateUI(bool isOn)
        {
            SetStatus(isOn ? "Open" : "Closed");
        }
    }
}
