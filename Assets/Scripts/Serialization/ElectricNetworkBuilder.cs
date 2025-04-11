using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Serialization
{
    public static class ElectricNetworkBuilder
    {
        public static Dictionary<string, IElectricNode> BuildFromAsset(ElectricNetworkAsset asset)
        {
            var nodes = new Dictionary<string, IElectricNode>();

            // 1. Создание всех нод
            foreach (var def in asset.devices)
            {
                var id = new DeviceId(def.id);
                nodes[def.id] = CreateNode(def.type, id, def.energyRequired, def.useDuration);
            }

            // 2. Подключение связей: входы и выходы
            foreach (var def in asset.devices)
            {
                var current = nodes[def.id];

                foreach (var inputId in def.inputs)
                {
                    if (!nodes.TryGetValue(inputId, out var input))
                        continue;

                    if (current is IInputAccepting inputAccepting)
                        inputAccepting.ConnectInput(input);

                    if (input is IOutputAccepting outputAccepting)
                    {
                        outputAccepting.ConnectOutput(current);
                    }
                }
            }

            return nodes;
        }


        public static IElectricNode CreateNode(ElectricDeviceType type, DeviceId id, float energyRequired, float useDuration)
        {
            return type switch
            {
                ElectricDeviceType.PowerSource => new PowerSource(id),
                ElectricDeviceType.ElectricSwitch => new ElectricSwitch(null, id),
                ElectricDeviceType.Lamp => new Lamp(null, id, energyRequired),
                ElectricDeviceType.Door => new DoorDrive(null, id, energyRequired, useDuration),
                ElectricDeviceType.AndGate => new ElectricNodeComposite(Logic.And, id),
                ElectricDeviceType.OrGate => new ElectricNodeComposite(Logic.Or, id),
                _ => throw new System.Exception($"Unsupported type: {type}")
            };
        }
    }
}
