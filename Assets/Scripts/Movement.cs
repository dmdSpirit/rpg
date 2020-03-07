using System;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public event Action<Unit> OnUnitBeginMoving;
    public event Action<Unit> OnUnitEndMoving;

    [SerializeField] private float proximityRange = .01f;

    private Unit unit;
    private NavMeshAgent navMeshAgent;
    private bool isMoving;
    private Vector3 destination;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = proximityRange + 0.01f;
    }

    private void FixedUpdate()
    {
        if (isMoving == false) return;
        if (navMeshAgent.remainingDistance <= proximityRange)
            FinishMovement();
    }

    public void StartMoving(Vector3 destination)
    {
        this.destination = destination;
        navMeshAgent.SetDestination(destination);
        navMeshAgent.isStopped = false;
        isMoving = true;
        AnimationSystem.Instance.StartAnimation();
        OnUnitBeginMoving?.Invoke(unit);
    }

    private void FinishMovement()
    {
        isMoving = false;
        navMeshAgent.isStopped = true;
        AnimationSystem.Instance.StopAnimation();
        OnUnitEndMoving?.Invoke(unit);
    }
}