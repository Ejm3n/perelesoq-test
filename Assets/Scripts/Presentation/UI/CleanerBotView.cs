using UnityEngine;
using UnityEngine.UI;
using SmartHome.Domain;
using TMPro;

namespace SmartHome.Presentation
{
    public class CleanerBotView : DeviceWidgetView
    {
        [SerializeField] private Button cleanBtn;
        [SerializeField] private TMP_Text buttonLabel;

        private CleanerBot _bot;

        public void Init(CleanerBot bot)
        {
            _bot = bot;
            SetName(bot.Id.Value);

            cleanBtn.onClick.AddListener(() =>
            {
                if (_bot.State == CleanerBotState.Patrolling)
                    _bot.CommandStop();
                else
                    _bot.CommandStartCleaning();
            });

            bot.OnStateChanged += OnStateChanged;
            OnStateChanged(bot.State);
        }

        private void OnStateChanged(CleanerBotState state)
        {
            SetStatus(state.ToString());

            switch (state)
            {
                case CleanerBotState.Patrolling:
                    buttonLabel.text = "Stop";
                    cleanBtn.interactable = true;
                    break;

                case CleanerBotState.Idle:
                case CleanerBotState.Charging:
                    buttonLabel.text = "Start";
                    cleanBtn.interactable = true;
                    break;

                case CleanerBotState.Returning:
                    buttonLabel.text = "Returning...";
                    cleanBtn.interactable = false;
                    break;
            }
        }
    }
}