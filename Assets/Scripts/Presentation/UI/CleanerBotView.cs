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
            switch (state)
            {
                case CleanerBotState.Patrolling:
                    SetStatus("Patrolling");
                    buttonLabel.text = "Stop";
                    cleanBtn.interactable = true;
                    break;

                case CleanerBotState.Idle:
                    SetStatus("Idle");
                    buttonLabel.text = "Start";
                    cleanBtn.interactable = true;
                    break;

                case CleanerBotState.Charging:
                    SetStatus(_bot.IsFullyCharged ? "Ready" : "Charging");
                    buttonLabel.text = "Start";
                    cleanBtn.interactable = true;
                    break;

                case CleanerBotState.Returning:
                    SetStatus("Returning");
                    buttonLabel.text = "Returning...";
                    cleanBtn.interactable = false;
                    break;
            }
        }
    }
}