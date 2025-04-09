using System;
using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;

namespace SmartHome.Application
{
    public sealed class SelectCameraUseCase
    {
        private readonly IDeviceRepository _repo;
        public CameraDevice ActiveCamera { get; private set; }
        public SelectCameraUseCase(IDeviceRepository repo) => _repo = repo;

        public void Execute(DeviceId id)
        {
            ActiveCamera = _repo.Get<CameraDevice>(id) ?? throw new ArgumentException("Camera not found");
        }
    }
}
