using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SmartHome.Presentation
{
    public abstract class DeviceWidgetView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private TMP_Text _nameText;
        protected void SetStatus(string txt) => _statusText.text = txt;
        //  protected void SetName(string txt) => _nameText.text = txt;
    }
}