using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    /// <summary>
    /// Интерфейс для любого элемента электрической цепи.
    /// </summary>
    public interface IElectricNode
    {
        bool HasCurrent { get; }
    }

}
