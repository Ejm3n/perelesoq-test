using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public interface ISwitchable
    {
        bool IsOn { get; }
        void Switch(bool state);
        event Action<bool> OnSwitch;
    }
}
