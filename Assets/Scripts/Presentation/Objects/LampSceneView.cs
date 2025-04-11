using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;



namespace SmartHome.Presentation
{
    public class LampSceneView : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private MeshRenderer LampMesh;
        [SerializeField] private Material LampOnMaterial;
        [SerializeField] private Material LampOffMaterial;

        void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        private void TryBind(DeviceId deviceId, IDevice device)
        {
            if (deviceId.Value != id) return;
            if (device is Lamp lamp)
            {
                lamp.OnSwitch += state => LampMesh.material = state ? LampOnMaterial : LampOffMaterial;
                LampMesh.material = lamp.IsOn ? LampOnMaterial : LampOffMaterial;
                DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
            }
        }

        void OnDestroy()
        {
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }
    }

}
