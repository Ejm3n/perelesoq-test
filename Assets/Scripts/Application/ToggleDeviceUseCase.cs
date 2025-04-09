using System;
using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Application
{
    public sealed class ToggleDeviceUseCase
    {
        private readonly IDeviceRepository _repo;
        public ToggleDeviceUseCase(IDeviceRepository repo) => _repo = repo;

        public void Execute(DeviceId id)
        {
            if (_repo.Get<IDevice>(id) is ISwitchable s)
                s.Switch(!s.IsOn);
            else
                throw new InvalidOperationException("Device is not switchable");
        }
    }
}
