using System;
using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using TMPro;
using UnityEngine;

namespace SmartHome.Presentation
{
    /// <summary>
    /// SceneView для источника питания — отображает время, мощность и расход энергии.
    /// </summary>
    public class PowerSourceSceneView : SceneViewBase<PowerSource>
    {
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private PowerSource _powerSource;

        protected override void OnDeviceBound(PowerSource power)
        {
            _powerSource = power;
            power.OnPowerChange += OnPowerChange;
            power.OnTimeChange += OnTimeChange;
            UpdateText();
        }

        private void OnPowerChange(float currentPower, float totalConsumedEnergy)
        {
            UpdateText();
        }

        private void OnTimeChange(float time)
        {
            UpdateText();
        }

        /// <summary>
        /// Обновляет текст в статусе.
        /// </summary>
        private void UpdateText()
        {
            _statusText.text = $"Time: {Utils.Utils.FormatTime(_powerSource.Time)}\nTotal: {_powerSource.TotalConsumedEnergy.ToString("F0")} W\nCurrent: {_powerSource.CurrentPower.ToString("F0")} W";
        }
    }
}
