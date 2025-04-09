using System.Collections;
using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;


namespace SmartHome.Presentation
{
    public sealed class CameraView : DeviceWidgetView
    {
        private CameraDevice _cam;
        private SelectCameraUseCase _select;
        public void Init(CameraDevice cam, SelectCameraUseCase sel)
        {
            _cam = cam; _select = sel;
            Refresh();
        }
        public void OnClicked()
        {
            _select.Execute(_cam.Id);
            Refresh();
        }
        private void Refresh() => SetStatus(_select.ActiveCamera == _cam ? "ACTIVE" : "- -");
    }
}