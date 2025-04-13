using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    /// <summary>
    /// Интерфейс для устройств, которые можно включать/выключать вручную,
    /// и которые умеют реагировать на изменение входного сигнала.
    /// </summary>
    public interface ISwitchable
    {
        bool IsOn { get; }
        void SwitchState(bool state);
        event Action<bool> OnSwitch;
        void RefreshState();
    }
}
