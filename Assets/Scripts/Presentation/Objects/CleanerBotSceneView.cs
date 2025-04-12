using UnityEngine;
using UnityEngine.AI;
using SmartHome.Domain;
using SmartHome.Presentation;

public class CleanerBotSceneView : SceneViewBase<CleanerBot>
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _dockPoint;
    [SerializeField] private float checkInterval = 0f;
    [SerializeField] private float randomRadius = 5f;

    private float _checkTimer;

    protected override void OnDeviceBound(CleanerBot bot)
    {
        _device = bot;
        bot.DockPosition = _dockPoint.position;
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

    private void Update()
    {
        if (_device == null || _agent.pathPending) return;

        _checkTimer += Time.deltaTime;
        if (_checkTimer >= checkInterval)
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
                    Debug.Log("Путь к базе пропал. Жду, пока снова откроется...");
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
                _device.Switch(false);
                _device.SetState(CleanerBotState.Charging);
            }
        }
    }

    private void TryPickRandomTarget()
    {
        Vector3 random = _agent.transform.position + Random.insideUnitSphere * randomRadius;
        if (NavMesh.SamplePosition(random, out var hit, randomRadius, NavMesh.AllAreas))
        {
            if (HasPath(_agent.transform.position, hit.position))
            {
                _agent.isStopped = false;
                _agent.SetDestination(hit.position);
                return;
            }
        }

        Debug.Log("Не удалось найти путь к случайной точке. Жду...");
    }

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
            Debug.Log("Путь к базе недоступен. Жду открытия.");
        }
    }

    private bool HasPath(Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();
        bool valid = NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
        return valid && path.status == NavMeshPathStatus.PathComplete;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_device != null)
            _device.OnStateChanged -= HandleStateChange;
    }
}
