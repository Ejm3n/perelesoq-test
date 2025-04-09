using System.Collections;
using System.Collections.Generic;
using SmartHome.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace SmartHome.Presentation
{
    public sealed class LampView : MonoBehaviour
    {
        // [SerializeField] private Image _icon;
        private Lamp _lamp;
        public void Init(Lamp lamp)
        {
            _lamp = lamp;
            // UpdateIcon();
        }
        // private void UpdateIcon() => _icon.color = _lamp.IsOn ? Color.yellow : Color.gray;
        // private void LateUpdate() => UpdateIcon();
    }
}
