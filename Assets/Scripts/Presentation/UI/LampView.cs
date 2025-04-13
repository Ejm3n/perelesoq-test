using SmartHome.Domain;


namespace SmartHome.Presentation
{
    /// <summary>
    /// Виджет для лампы. Показывает статус ON/OFF.
    /// </summary>
    public sealed class LampView : DeviceWidgetView
    {
        private Lamp _lamp;
        public void Init(Lamp lamp)
        {
            _lamp = lamp;
            if (_lamp != null)
                _lamp.OnSwitch += Refresh;

            SetName(lamp.Id.Value);
        }

        private void OnDisable()
        {
            if (_lamp != null)
                _lamp.OnSwitch -= Refresh;
        }

        private void Refresh(bool status) => SetStatus(status ? "ON" : "OFF");
    }
}
