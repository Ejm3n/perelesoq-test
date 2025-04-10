using System;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Serialization
{
    [Serializable]
    public sealed class ElectricDeviceDefinition
    {
        public string id;
        public ElectricDeviceType type;
        public List<string> inputs;
        public List<string> outputs;
        public float posX;
        public float posY;

    }
}
