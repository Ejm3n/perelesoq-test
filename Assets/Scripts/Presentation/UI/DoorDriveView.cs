using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public sealed class DoorDriveView : DeviceWidgetView
    {
        private DoorDrive _door;

        public void Init(DoorDrive door, string name)
        {
            _door = door;
            SetName(name);
            Refresh();
            _door.OnSwitch += _ => Refresh();
        }

        private void Refresh()
        {
            if (_door.IsMoving)
                SetStatus("opening...");
            else
                SetStatus(_door.IsOpen ? "open" : "closed");
        }

        private void OnDisable()
        {
            if (_door != null)
                _door.OnSwitch -= _ => Refresh();
        }

        public void OnOpen()
        {
            if (!_door.IsMoving)
                _door.Switch(true);
        }

        public void OnClose()
        {
            if (!_door.IsMoving)
                _door.Switch(false);
        }
    }
}
