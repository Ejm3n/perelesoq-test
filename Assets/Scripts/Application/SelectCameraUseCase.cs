using System.Collections.Generic;
using SmartHome.Domain;

namespace SmartHome.Application
{
    public sealed class SelectCameraUseCase
    {
        private readonly List<CameraDevice> _allCameras;

        public SelectCameraUseCase(List<CameraDevice> cameras)
        {
            _allCameras = cameras;
        }

        /// <summary>
        /// Делает активной только выбранную камеру, выключая все остальные.
        /// </summary>
        public void Select(CameraDevice target)
        {
            foreach (var cam in _allCameras)
            {
                if (cam == target) cam.Select();
                else cam.Deselect();
            }
        }
    }
}
