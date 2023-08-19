using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleState : State
{
    private float time;
    private float timeToWait;

    private EnemyController ec;
    private AIBehavior AIBehavior;

    public IdleState(AIBehavior AIBehavior) : base("Idle State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        time = 0f;
        timeToWait = Random.Range(ec.minTimeIdle, ec.maxTimeIdle);

        ec.SetMovePower(0f);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (AIBehavior.enemyController.playerInSightRange) AIBehavior.ChangeState(AIBehavior.chaseState);
        else
        {
            if (time < timeToWait) time += Time.deltaTime;
            else AIBehavior.ChangeState(AIBehavior.patrolState);
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        time = 0f;
    }
}