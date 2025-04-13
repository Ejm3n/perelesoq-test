using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    /// <summary>
    /// Интерфейс для устройств, которые могут отдавать сигнал на выход (ток).
    /// </summary>
    public interface IOutputAccepting
    {
        void ConnectOutput(IElectricNode output);
    }
}