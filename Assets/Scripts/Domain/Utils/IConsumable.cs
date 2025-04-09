using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public interface IConsumable : ISwitchable
    {
        /// <summary>Rated power in Watts when device is ON.</summary>
        float RatedPower { get; }
        /// <summary>Accumulated kWh.</summary>
        float ConsumedEnergy { get; }
    }
}
