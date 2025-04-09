using SmartHome.Domain;


namespace SmartHome.Presentation
{
    public sealed class DoorDriveView : DeviceWidgetView
    {
        private DoorDrive _door;
        public void Init(DoorDrive door)
        {
            _door = door;
            Refresh();
        }
        public void OnOpen() { _door.Switch(true); Refresh(); }
        public void OnClose() { _door.Switch(false); Refresh(); }
        private void Refresh() => SetStatus(_door.IsOn ? "BUSY" : "IDLE");
    }
}