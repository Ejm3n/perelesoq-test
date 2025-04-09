using SmartHome.Domain;


namespace SmartHome.Presentation
{
    public sealed class LampView : DeviceWidgetView
    {
        private Lamp _lamp;
        public void Init(Lamp lamp)
        {
            _lamp = lamp;
            Refresh();
        }

        private void Refresh() => SetStatus(_lamp.IsOn ? "ON" : "OFF");
    }
}
