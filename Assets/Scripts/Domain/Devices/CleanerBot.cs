using System;
using UnityEngine;

namespace SmartHome.Domain
{
    public enum CleanerBotState
    {
        Idle,
        Charging,
        Patrolling,
        Returning
    }
    public sealed class CleanerBot : IDevice, IConsumable, IInputAccepting, IElectricNode
    {
        private IElectricNode _input;

        public DeviceId Id { get; }
        public bool IsOn { get; private set; }

        public float BatteryCapacity { get; } = 60f;
        public float BatteryLevel { get; private set; } = 60f;
        public float DrainRate { get; } = 1.0f; // разрядка
        public float ChargeRate { get; } = 2.0f; // скорость зарядки

        public float RatedPower => IsOn ? DrainRate : 0f;
        public float ConsumedEnergy { get; private set; }
        public CleanerBotState State { get; private set; }

        public Vector3 DockPosition { get; set; } // передаётся из сценвью

        public event Action<CleanerBotState> OnStateChanged;
        public bool HasCurrent => BatteryLevel > 0f;

        public event Action<bool> OnSwitch;

        public CleanerBot(DeviceId id)
        {
            Id = id;
        }
        public void CommandStartCleaning()
        {
            if (BatteryLevel > 0)
                SetState(CleanerBotState.Patrolling);
        }

        public void CommandStop()
        {
            if (State == CleanerBotState.Patrolling)
            {
                Switch(false);
                SetState(CleanerBotState.Idle);
            }
        }

        public void Switch(bool state)
        {
            IsOn = state;
            OnSwitch?.Invoke(state);
        }

        public void Tick(float deltaTime)
        {
            switch (State)
            {
                case CleanerBotState.Patrolling:
                    BatteryLevel -= DrainRate * deltaTime;
                    BatteryLevel = Mathf.Max(BatteryLevel, 0f);

                    if (BatteryLevel <= 0f)
                        SetState(CleanerBotState.Returning);
                    break;

                case CleanerBotState.Charging:
                    if (_input?.HasCurrent == true && BatteryLevel < BatteryCapacity)
                    {
                        BatteryLevel += ChargeRate * deltaTime;
                        BatteryLevel = Mathf.Min(BatteryLevel, BatteryCapacity);
                    }
                    break;
            }
        }

        public void SetState(CleanerBotState newState)
        {
            if (State == newState) return;
            State = newState;
            OnStateChanged?.Invoke(State);
        }
        public void ConnectInput(IElectricNode input) => _input = input;

        public void RefreshState() { } // необязательно
    }
}
