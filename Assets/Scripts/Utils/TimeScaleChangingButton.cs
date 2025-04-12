using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SmartHome.Utils
{
    public class TimeScaleChangingButton : MonoBehaviour
    {
        [SerializeField] private float _timeScale = 1f;
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            Utils.ChangeTimeScale(_timeScale);
        }
    }
}