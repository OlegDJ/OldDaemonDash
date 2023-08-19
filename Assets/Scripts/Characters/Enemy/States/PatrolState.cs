using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private bool patrolPointSet;
    private Vector3 patrolPointPosition;
    private float distanceFromPatrolPoint;

    private EnemyController ec;
    private AIBehavior AIBehavior;

    public PatrolState(AIBehavior AIBehavior) : base("Patrol State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        patrolPointSet = false;

        patrolPointPosition = GetPatrolPointPosition();

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(patrolPointPosition, out navMeshHit, ec.maxPatrolRange, -1))
        {
            patrolPointSet = true;

            patrolPointPosition = new Vector3(navMeshHit.position.x, navMeshHit.position.y + ec.offsetFromObjectCenter.y, navMeshHit.position.z);
            ec.SetDestinationPosition(patrolPointPosition);

            ec.SetNavMeshAgentSpeed(true);
            ec.SetMovePower(0.5f);
        }
        else AIBehavior.ChangeState(AIBehavior.idleState);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (ec.playerInSightRange) AIBehavior.ChangeState(AIBehavior.chaseState);
        else
        {
            if (patrolPointSet)
            {
                distanceFromPatrolPoint = Vector3.Distance(ec.modifiedPosition, patrolPointPosition);

                if (distanceFromPatrolPoint <= ec.minDistanceToStop) AIBehavior.ChangeState(AIBehavior.idleState);
            }
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        patrolPointSet = false;
    }

    private Vector3 GetPatrolPointPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        return ec.modifiedPosition + randomDirection * Random.Range(ec.minPatrolRange, ec.maxPatrolRange);
    }
}