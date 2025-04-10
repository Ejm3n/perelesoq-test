using System.Collections;
using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Presentation
{
    public sealed class WidgetFactory : MonoBehaviour
    {
        private Transform _root;
        private LampView _lampPrefab;
        private SwitchView _switchPrefab;
        private DoorDriveView _doorPrefab;
        private CameraView _cameraPrefab;
        private SelectCameraUseCase _selectCameraUseCase;


    }

}
