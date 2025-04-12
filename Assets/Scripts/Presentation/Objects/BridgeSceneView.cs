using UnityEngine;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    public class BridgeSceneView : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private MeshRenderer targetRenderer;
        [SerializeField] private Material activeMat;
        [SerializeField] private Material inactiveMat;

        void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        void TryBind(DeviceId deviceId, IDevice device)
        {
            if (deviceId.Value != id || device is not Bridge bridge) return;

            bridge.OnSwitch += state =>
                targetRenderer.material = state ? activeMat : inactiveMat;

            targetRenderer.material = bridge.HasCurrent ? activeMat : inactiveMat;
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }

        void OnDestroy()
        {
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }
    }
}