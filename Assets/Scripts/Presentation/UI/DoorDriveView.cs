using SmartHome.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmartHome.Presentation
{
    public sealed class DoorDriveView : DeviceWidgetView
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _buttonStatusText;
        private DoorDrive _door;


        public void Init(DoorDrive door, string name)
        {
            _door = door;
            SetName(name);
            Refresh();
            _door.OnSwitch += _ => Refresh();
            _button.onClick.AddListener(ToggleDoor);
        }

        private void Refresh()
        {
            if (!_door.HasCurrent)
            {
                _button.interactable = false;
                _buttonStatusText.text = "no power";
                return;
            }

            if (_door.IsMoving)
            {
                _button.interactable = false;
                SetStatus("executing...");
                _buttonStatusText.text = "executing...";
            }
            else
            {
                _button.interactable = true;
                SetStatus(_door.IsOpen ? "open" : "closed");
                _buttonStatusText.text = !_door.IsOpen ? "open" : "close";
            }
        }

        private void OnDisable()
        {
            if (_door != null)
                _door.OnSwitch -= _ => Refresh();
        }

        private void ToggleDoor()
        {
            if (_door.IsMoving || !_door.HasCurrent)
                return;
            _door.Switch(!_door.IsOpen);
        }

    }
}
