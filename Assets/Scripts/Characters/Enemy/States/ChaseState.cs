using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    private EnemyController ec;
    private AIBehavior AIBehavior;

    public ChaseState(AIBehavior AIBehavior) : base("Chase State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        ec.SetNavMeshAgentSpeed(false);
        ec.SetMovePower(1f);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (ec.playerInSightRange) ec.SetDestinationPosition(ec.modifiedPlayerPosition);
        else AIBehavior.ChangeState(AIBehavior.idleState);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}