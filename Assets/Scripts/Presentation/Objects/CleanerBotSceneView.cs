using UnityEngine;
using UnityEngine.AI;
using SmartHome.Domain;

namespace SmartHome.Presentation
{
    /// <summary>
    /// SceneView для пылесоса: управляет перемещением и навигацией.
    /// </summary>
    public class CleanerBotSceneView : SceneViewBase<CleanerBot>
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Transform _dockPoint;
        [SerializeField] private float _checkInterval = 0f;
        [SerializeField] private float _randomRadius = 5f;

        private float _checkTimer;

        /// <summary>
        /// Проверяет путь на базу, реагирует на смену состояния и завершение движения.
        /// </summary>
        private void Update()
        {
            if (_device == null || _agent.pathPending) return;

            _checkTimer += Time.deltaTime;
            if (_checkTimer >= _checkInterval)
            {
                _checkTimer = 0f;

                if (_device.State == CleanerBotState.Returning)
                {
                    // Если он стоит (т.к. путь был недоступен), проверим ещё раз
                    if (_agent.isStopped)
                    {
                        TrySetDockAsTarget(); // попытаемся снова построить путь
                    }
                    // Если путь внезапно стал неполным — остановим
                    else if (!_agent.hasPath || _agent.pathStatus != NavMeshPathStatus.PathComplete)
                    {
                        _agent.isStopped = true;
                    }
                }
            }

            // Обработка завершения пути
            if (!_agent.pathPending && !_agent.isStopped && _agent.remainingDistance < 0.2f)
            {
                if (_device.State == CleanerBotState.Patrolling)
                {
                    TryPickRandomTarget();
                }
                else if (_device.State == CleanerBotState.Returning)
                {
                    _device.SwitchState(false);
                    _device.SetState(CleanerBotState.Charging);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_device != null)
                _device.OnStateChanged -= HandleStateChange;
        }

        protected override void OnDeviceBound(CleanerBot bot)
        {
            _device = bot;
            bot.OnStateChanged += HandleStateChange;
            HandleStateChange(bot.State);
        }

        private void HandleStateChange(CleanerBotState state)
        {
            _agent.isStopped = true;

            switch (state)
            {
                case CleanerBotState.Idle:
                case CleanerBotState.Charging:
                    break;

                case CleanerBotState.Patrolling:
                    TryPickRandomTarget();
                    break;

                case CleanerBotState.Returning:
                    TrySetDockAsTarget();
                    break;
            }
        }

        /// <summary>
        /// Выбирает случайную точку в зоне патрулирования и отправляет туда бота.
        /// </summary>
        private void TryPickRandomTarget()
        {
            Vector3 random = _agent.transform.position + Random.insideUnitSphere * _randomRadius;
            if (NavMesh.SamplePosition(random, out var hit, _randomRadius, NavMesh.AllAreas))
            {
                if (HasPath(_agent.transform.position, hit.position))
                {
                    _agent.isStopped = false;
                    _agent.SetDestination(hit.position);
                    return;
                }
            }
        }

        /// <summary>
        /// Пробует отправить бота на базу, если путь доступен.
        /// </summary>
        private void TrySetDockAsTarget()
        {
            Vector3 dockPos = _dockPoint.position;
            if (HasPath(_agent.transform.position, dockPos))
            {
                _agent.isStopped = false;
                _agent.SetDestination(dockPos);
            }
            else
            {
                _agent.isStopped = true;
            }
        }

        private bool HasPath(Vector3 from, Vector3 to)
        {
            NavMeshPath path = new NavMeshPath();
            bool valid = NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
            return valid && path.status == NavMeshPathStatus.PathComplete;
        }
    }
}