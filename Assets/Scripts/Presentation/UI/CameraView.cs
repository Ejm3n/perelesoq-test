using System.Collections;
using System.Collections.Generic;
using SmartHome.Application;
using SmartHome.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SmartHome.Presentation
{
    /// <summary>
    /// Виджет для выбора камеры. Отображает состояние и запускает UseCase.
    /// </summary>
    public class CameraView : DeviceWidgetView
    {
        [SerializeField] private Button _selectButton;
        private CameraDevice _device;
        private SelectCameraUseCase _useCase;

        public void Init(CameraDevice device, SelectCameraUseCase useCase)
        {
            _device = device;
            _useCase = useCase;

            SetName(_device.Id.Value);
            UpdateUI();

            _selectButton.onClick.AddListener(OnSelectClick);
            _device.OnSelected += _ => UpdateUI();
            if (_device.IsSelected)
                OnSelectClick();
        }

        public void OnSelectClick()
        {
            _useCase.Select(_device);
        }

        private void UpdateUI()
        {
            bool selected = _device.IsSelected;

            SetStatus(selected ? "Selected" : "Select");
            _selectButton.interactable = !selected;
        }

        private void OnDestroy()
        {
            if (_device != null)
                _device.OnSelected -= _ => UpdateUI();
        }
    }
}