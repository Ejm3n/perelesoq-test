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

    /// <summary>
    /// Робот-пылесос с батареей, умеет патрулировать, заряжаться и возвращаться.
    /// </summary>
    public sealed class CleanerBot : IDevice, IConsumable, IInputAccepting, IElectricNode
    {
        public DeviceId Id { get; }
        public bool IsOn { get { return State == CleanerBotState.Charging && !WasFullyCharged; } }
        public float BatteryCapacity { get; }
        public float BatteryLevel { get; private set; }
        public float DrainRate { get; }
        public float ChargeRate { get; }
        public float RatedPower
        {
            get
            {
                if (State == CleanerBotState.Charging && _input?.HasCurrent == true)
                    return BatteryCapacity;

                return 0f;
            }
        }

        public float ConsumedEnergy { get; private set; }
        public CleanerBotState State { get; private set; }
        public bool IsFullyCharged => Mathf.Approximately(BatteryLevel, BatteryCapacity);
        public event Action<CleanerBotState> OnStateChanged;
        public bool HasCurrent => BatteryLevel > 0f;
        public bool WasFullyCharged { get; private set; }
        public event Action<bool> OnSwitch;
        private IElectricNode _input;

        public CleanerBot(DeviceId id, float batteryCapacity, float drainRate, float chargeRate)
        {
            Id = id;
            BatteryCapacity = batteryCapacity;
            BatteryLevel = batteryCapacity;
            DrainRate = drainRate;
            ChargeRate = chargeRate;
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

        public int GetCurrentBatteryLevelPercent()
        {
            return (int)(BatteryLevel / BatteryCapacity * 100);
        }

        public void Switch(bool state)
        {
            //IsOn = state;
            OnSwitch?.Invoke(state);
        }

        public void Tick(float deltaTime)
        {
            switch (State)
            {
                case CleanerBotState.Patrolling: // патрулирование, батарея разряжается
                    BatteryLevel -= DrainRate * deltaTime;
                    BatteryLevel = Mathf.Max(BatteryLevel, 0f);

                    if (BatteryLevel <= 0f)
                        SetState(CleanerBotState.Returning);
                    break;

                case CleanerBotState.Charging:
                    if (_input?.HasCurrent == true)
                    {
                        float maxDelta = BatteryCapacity - BatteryLevel;
                        float actualDelta = Mathf.Min(ChargeRate * deltaTime, maxDelta);

                        BatteryLevel += actualDelta;
                        ConsumedEnergy += actualDelta;

                        bool nowFullyCharged = Mathf.Approximately(BatteryLevel, BatteryCapacity);
                        if (!WasFullyCharged && nowFullyCharged)
                        {
                            WasFullyCharged = true;
                            OnStateChanged?.Invoke(CleanerBotState.Charging);
                        }
                        else if (!nowFullyCharged)
                        {
                            WasFullyCharged = false;
                        }
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
