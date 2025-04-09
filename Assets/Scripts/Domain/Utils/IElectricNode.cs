using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public interface IElectricNode
    {
        bool HasCurrent { get; }
    }
}
