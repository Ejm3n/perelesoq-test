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
                nodes[def.id] = CreateNode(def.type);
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
                        Debug.Log($"[Builder] {input.GetType().Name} connects to {current.GetType().Name}");
                        outputAccepting.ConnectOutput(current);
                    }

                }
            }

            return nodes;
        }


        public static IElectricNode CreateNode(ElectricDeviceType type)
        {
            return type switch
            {
                ElectricDeviceType.PowerSource => new PowerSource(),
                ElectricDeviceType.ElectricSwitch => new ElectricSwitch(null),
                ElectricDeviceType.Lamp => new Lamp(null),
                ElectricDeviceType.Door => new DoorDrive(null),
                ElectricDeviceType.AndGate => new ElectricNodeComposite(Logic.And),
                ElectricDeviceType.OrGate => new ElectricNodeComposite(Logic.Or),
                _ => throw new System.Exception($"Unsupported type: {type}")
            };
        }
    }
}
