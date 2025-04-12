using UnityEngine;
using UnityEngine.UI;

namespace SmartHome.Utils
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(Utils.ReloadAllOpenScenes);
        }
    }
}
