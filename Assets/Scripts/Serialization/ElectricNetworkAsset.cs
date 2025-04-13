using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Serialization
{
    /// <summary>
    /// ScriptableObject, описывающий электрическую сеть.
    /// Хранит список устройств и их связи. Граф сохраняется в такой ассет.
    /// </summary>

    [CreateAssetMenu(menuName = "Electric/ElectricNetworkAsset")]
    public class ElectricNetworkAsset : ScriptableObject
    {
        public List<ElectricDeviceDefinition> devices = new();
    }
}
