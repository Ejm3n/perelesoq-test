using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Serialization
{
    public static class ElectricNetworkBuilder
    {
        public static Dictionary<string, IElectricNode> BuildFromAsset(ElectricNetworkAsset asset,
            IDeviceRepository repo,
            List<CameraDevice> cameraDevices)
        {
            var nodes = new Dictionary<string, IElectricNode>();

            foreach (var def in asset.devices)
            {
                var id = new DeviceId(def.id);
                var node = CreateNode(def.type, id, def.energyRequired, def.useDuration);

                if (node is IDevice device)
                {
                    repo.Add(device);
                    DeviceFactoryNotifier.Notify(id, device);
                }

                if (node is CameraDevice cam)
                    cameraDevices.Add(cam);

                if (node is IElectricNode electricNode)
                    nodes[def.id] = electricNode;
            }

            // Подключение связей
            foreach (var def in asset.devices)
            {
                if (!nodes.TryGetValue(def.id, out var current))
                    continue;

                foreach (var inputId in def.inputs)
                {
                    if (!nodes.TryGetValue(inputId, out var input))
                        continue;

                    if (current is IInputAccepting inputAccepting)
                        inputAccepting.ConnectInput(input);

                    if (input is IOutputAccepting outputAccepting)
                        outputAccepting.ConnectOutput(current);
                }
            }

            return nodes;
        }

        public static object CreateNode(ElectricDeviceType type, DeviceId id, float energyRequired, float useDuration)
        {
            return type switch
            {
                ElectricDeviceType.PowerSource => new PowerSource(id),
                ElectricDeviceType.ElectricSwitch => new ElectricSwitch(null, id),
                ElectricDeviceType.Lamp => new Lamp(null, id, energyRequired),
                ElectricDeviceType.Door => new DoorDrive(null, id, energyRequired, useDuration),
                ElectricDeviceType.AndGate => new ElectricNodeComposite(Logic.And, id),
                ElectricDeviceType.OrGate => new ElectricNodeComposite(Logic.Or, id),
                ElectricDeviceType.Camera => new CameraDevice(id),
                _ => throw new System.Exception($"Unsupported device type: {type}")
            };
        }
    }
}
