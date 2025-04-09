using System.Collections;
using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Presentation
{
    public sealed class WidgetFactory : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private LampView _lampPrefab;
        [SerializeField] private SwitchView _switchPrefab;
        [SerializeField] private DoorDriveView _doorPrefab;
        [SerializeField] private CameraView _cameraPrefab;
        private SelectCameraUseCase _selectCamera;

        public void Configure(Transform root,
                              LampView lampPf,
                              SwitchView swPf,
                              DoorDriveView doorPf,
                              CameraView camPf)
        {
            _root = root; _lampPrefab = lampPf; _switchPrefab = swPf;
            _doorPrefab = doorPf; _cameraPrefab = camPf;
        }

        public void Build(IEnumerable<Domain.IDevice> devices)
        {
            // locate SelectCameraUseCase if present in scene
            _selectCamera = FindObjectOfType<Infrastructure.Bootstrap>()
                            .GetComponent<Infrastructure.Bootstrap>()
                            ?.GetComponent<SelectCameraUseCase>();

            foreach (var d in devices)
            {
                switch (d)
                {
                    case Lamp lamp:
                        var lv = Instantiate(_lampPrefab, _root);
                        lv.Init(lamp);
                        break;
                    case Domain.ElectricSwitch sw:
                        var sv = Instantiate(_switchPrefab, _root);
                        // sv.Init(sw);
                        break;
                    case DoorDrive door:
                        var dv = Instantiate(_doorPrefab, _root);
                        dv.Init(door);
                        break;
                    case CameraDevice cam when _cameraPrefab != null:
                        var cv = Instantiate(_cameraPrefab, _root);
                        cv.Init(cam, _selectCamera);
                        break;
                }
            }
        }
    }
}
