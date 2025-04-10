using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Serialization
{
    [CreateAssetMenu(menuName = "Electric/ElectricNetworkAsset")]
    public class ElectricNetworkAsset : ScriptableObject
    {
        public List<ElectricDeviceDefinition> devices = new();
    }
}
