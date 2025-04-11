using System;
using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using TMPro;
using UnityEngine;

namespace SmartHome.Presentation
{
    public class PowerSourceSceneView : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private PowerSource _powerSource;

        void Awake()
        {
            DeviceFactoryNotifier.OnDeviceCreated += TryBind;
        }

        private void TryBind(DeviceId deviceId, IDevice device)
        {
            if (deviceId.Value != id) return;
            if (device is PowerSource powerSource)
            {
                _powerSource = powerSource;
                _powerSource.OnPowerChange += OnPowerChange;
                _powerSource.OnTimeChange += OnTimeChange;

                DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
            }
        }

        private void OnPowerChange(float currentPower, float totalConsumedEnergy)
        {
            UpdateText();
        }

        private void OnTimeChange(float time)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            _statusText.text = $"Time: {Utils.Utils.FormatTime(_powerSource.Time)}\nTotal: {_powerSource.TotalConsumedEnergy.ToString("F1")} W\nCurrent: {_powerSource.CurrentPower.ToString("F1")} W";
        }

        void OnDestroy()
        {
            DeviceFactoryNotifier.OnDeviceCreated -= TryBind;
        }
    }
}
