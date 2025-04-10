using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Serialization
{
    public static class ElectricGraphLoader
    {
        public static Dictionary<string, IElectricNode> LoadFromAsset(ElectricNetworkAsset asset)
        {
            var nodes = new Dictionary<string, IElectricNode>();

            // 1. Создаём все узлы
            foreach (var def in asset.devices)
            {
                nodes[def.id] = ElectricNetworkBuilder.CreateNode(def.type);
            }

            // 2. Соединяем входы/выходы
            foreach (var def in asset.devices)
            {
                var current = nodes[def.id];

                foreach (var inputId in def.inputs)
                {
                    if (!nodes.TryGetValue(inputId, out var inputNode))
                        continue;

                    if (current is IInputAccepting acceptsInput)
                        acceptsInput.ConnectInput(inputNode);

                    if (inputNode is IOutputAccepting acceptsOutput)
                        acceptsOutput.ConnectOutput(current);
                }
            }

            return nodes;
        }
    }
}