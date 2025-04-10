using SmartHome.Domain;


namespace SmartHome.Presentation
{
    public sealed class LampView : DeviceWidgetView
    {
        private Lamp _lamp;
        public void Init(Lamp lamp)
        {
            _lamp = lamp;
            if (_lamp != null)
                _lamp.OnSwitch += Refresh;
        }

        private void OnDisable()
        {
            if (_lamp != null)
                _lamp.OnSwitch -= Refresh;
        }

        private void Refresh(bool status) => SetStatus(status ? "ON" : "OFF");
    }
}
