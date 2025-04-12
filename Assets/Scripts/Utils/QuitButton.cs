using UnityEngine;
using UnityEngine.UI;

namespace SmartHome.Utils
{
    public class QuitButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(Utils.QuitGame);
        }
    }
}
