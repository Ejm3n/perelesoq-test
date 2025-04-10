using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Domain
{
    public interface IOutputAccepting
    {
        void ConnectOutput(IElectricNode output);
    }
}