using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using TMPro;
using UnityEngine;

namespace SmartHome.Presentation
{
    public class PowerSourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentPowerText;
        [SerializeField] private TMP_Text _totalConsumedEnergyText;
        [SerializeField] private TMP_Text _timeText;
        private PowerSource _power;

        public void Init(PowerSource power)
        {
            _power = power;
            _power.OnPowerChange += Refresh;
            _power.OnTimeChange += RefreshTime;
        }

        private void OnDisable()
        {
            _power.OnPowerChange -= Refresh;
            _power.OnTimeChange -= RefreshTime;
        }

        private void Refresh(float currentPower, float totalConsumedEnergy)
        {
            _currentPowerText.text = $"CURRENT: {currentPower} W";
            _totalConsumedEnergyText.text = $"TOTAL: {totalConsumedEnergy} W";
        }

        private void RefreshTime(float time)
        {
            _timeText.text = SmartHome.Utils.Utils.FormatTime(time);
        }

    }
}