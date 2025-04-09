using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartHome.Infrastructure
{
    public sealed class SimulationLoop : MonoBehaviour
    {
        private Application.IDeviceRepository _repo;
        public void Init(Application.IDeviceRepository repo) => _repo = repo;
        private void Update()
        {
            if (_repo == null) return;
            float dt = Time.deltaTime;
            foreach (var d in _repo.All)
                d.Tick(dt);
        }
    }
}
