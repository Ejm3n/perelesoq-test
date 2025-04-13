using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SmartHome.Domain
{
    /// <summary>
    /// Интерфейс для устройств, которые могут принимать входной сигнал (ток).
    /// </summary>
    public interface IInputAccepting
    {
        void ConnectInput(IElectricNode input);
    }
}