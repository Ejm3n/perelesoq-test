using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Serialization
{
    /// <summary>
    /// Режим потребления энергии устройством. В основном используется для Граф редактора. Возможно, в будущем будет использоваться для других целей.
    /// </summary>
    public enum EnergyConsumptionMode
    {
        None,
        PerTick,
        PerUse,
        BatteryPowered
    }
}
