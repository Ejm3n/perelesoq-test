using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Serialization
{
    /// <summary>
    /// Строит электрическую сеть по ассету: создает ноды, соединяет связи.
    /// </summary>
    public static class ElectricNetworkBuilder
    {
        /// <summary>
        /// Создает и связывает устройства на основе ScriptableObject.
        /// </summary>
        public static Dictionary<string, IElectricNode> BuildFromAsset(ElectricNetworkAsset asset,
            IDeviceRepository repo,
            List<CameraDevice> cameraDevices)
        {
            var nodes = new Dictionary<string, IElectricNode>();

            foreach (var def in asset.devices)
            {
                var id = new DeviceId(def.id);
                var node = CreateNode(def.type, id, def.energyRequired, def.useDuration, def.batteryCapacity, def.drainPerSecond, def.chargePerSecond);

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

        /// <summary>
        /// Создает конкретную реализацию устройства по типу.
        /// </summary>
        public static object CreateNode(ElectricDeviceType type, DeviceId id, float energyRequired, float useDuration, float batteryCapacity,
         float drainPerSecond, float chargePerSecond)
        {
            return type switch
            {
                ElectricDeviceType.PowerSource => new PowerSource(id),
                ElectricDeviceType.ElectricSwitch => new ElectricSwitch(null, id),
                ElectricDeviceType.Lamp => new Lamp(null, id, energyRequired),
                ElectricDeviceType.Door => new DoorDrive(null, id, energyRequired, useDuration),
                ElectricDeviceType.AndGate => new GateAnd(id),
                ElectricDeviceType.OrGate => new GateOr(id),
                ElectricDeviceType.Camera => new CameraDevice(id),
                ElectricDeviceType.Bridge => new Bridge(id),
                ElectricDeviceType.CleanerBot => new CleanerBot(id, batteryCapacity, drainPerSecond, chargePerSecond),
                ElectricDeviceType.ChargingStation => new ChargingStation(id),
                _ => throw new System.Exception($"Unsupported device type: {type}")
            };
        }
    }
}
