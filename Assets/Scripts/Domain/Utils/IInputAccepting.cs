using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SmartHome.Domain
{
    public interface IInputAccepting
    {
        void ConnectInput(IElectricNode input);
    }
}