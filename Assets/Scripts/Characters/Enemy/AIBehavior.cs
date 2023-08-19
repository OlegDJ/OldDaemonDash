using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class AIBehavior : StateMachine
{
    [HideInInspector] public EnemyController enemyController;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();

        idleState = new IdleState(this);
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
    }

    protected override State GetInitialState() { return idleState; }
}
