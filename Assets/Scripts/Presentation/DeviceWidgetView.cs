using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SmartHome.Presentation
{
    public abstract class DeviceWidgetView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statusText;
        protected void SetStatus(string txt) => _statusText.text = txt;
    }
}