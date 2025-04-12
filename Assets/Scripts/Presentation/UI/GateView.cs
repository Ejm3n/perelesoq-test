using UnityEngine;
using UnityEngine.UI;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public class GateView : DeviceWidgetView
    {

        public void Init(LogicGate gate, string displayName)
        {
            SetName(displayName);
            UpdateUI(gate.IsOn);

            gate.OnSwitch += UpdateUI;
        }

        private void UpdateUI(bool isOn)
        {
            SetStatus(isOn ? "Open" : "Closed");
        }
    }
}
