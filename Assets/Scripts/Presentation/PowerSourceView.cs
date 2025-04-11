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
            // Обновляем отдельные текстовые поля
            _currentPowerText.text = $"CURRENT: {currentPower} W";
            _totalConsumedEnergyText.text = $"TOTAL: {totalConsumedEnergy} kWh";
        }

        private void RefreshTime(float time)
        {
            // Обновляем текст времени
            _timeText.text = FormatTime(time);
        }

        // TODO: move to utils - вынести в отдельный класс с общими функциями
        private string FormatTime(float timeInSeconds)
        {
            // Преобразуем секунды в дни, часы, минуты и секунды
            int days = (int)(timeInSeconds / 86400); // 86400 seconds in a day
            int hours = (int)((timeInSeconds % 86400) / 3600);
            int minutes = (int)((timeInSeconds % 3600) / 60);
            int seconds = (int)(timeInSeconds % 60);

            // Возвращаем строку в формате "Xd Yh Zm Ws"
            return $"{days}d {hours}h {minutes}m {seconds}s";
        }
    }
}