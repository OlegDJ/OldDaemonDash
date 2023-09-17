using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class IdleState : State
{
    private float time;
    private float timeToWait;

    private EnemyController ec;
    private EnemyAIBehavior AIBehavior;

    public IdleState(EnemyAIBehavior AIBehavior) : base("Idle State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        time = 0f;
        timeToWait = Random.Range(ec.minTimeIdle, ec.maxTimeIdle);

        ec.SetNavMeshAgent(ec.modifiedPosition, ec.walkSpeed, false);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (ec.playerInSightRange) AIBehavior.ChangeState(AIBehavior.chaseState);
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

public class PatrolState : State
{
    private bool patrolPointSet;
    private Vector3 patrolPointPosition;
    private float distanceFromPatrolPoint;

    private EnemyController ec;
    private EnemyAIBehavior AIBehavior;

    public PatrolState(EnemyAIBehavior AIBehavior) : base("Patrol State", AIBehavior)
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
            ec.SetNavMeshAgent(patrolPointPosition, ec.walkSpeed, false);
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

                if (distanceFromPatrolPoint <= ec.minDistance || ec.navMeshAgent.desiredVelocity.magnitude <= ec.minVelocityToStop)
                {
                    AIBehavior.ChangeState(AIBehavior.idleState);
                }
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

public class ChaseState : State
{
    private EnemyController ec;
    private EnemyAIBehavior AIBehavior;

    public ChaseState(EnemyAIBehavior AIBehavior) : base("Chase State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (ec.playerInSightRange)
        {
            ec.SetNavMeshAgent(ec.modifiedPlayerPosition, ec.runSpeed, true);

            if (ec.distanceToPlayer <= ec.battleRadius) AIBehavior.ChangeState(AIBehavior.battleState);
        }
        else AIBehavior.ChangeState(AIBehavior.idleState);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}

public class BattleState : State
{
    private Vector3 positionOnBattleCircle;

    private float time;
    private float timeToWait;

    private EnemyController ec;
    private EnemyAIBehavior AIBehavior;

    public BattleState(EnemyAIBehavior AIBehavior) : base("Battle State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        time = 0f;
        timeToWait = Random.Range(ec.minTimeBetweenActions, ec.maxTimeBetweenActions);

        ec.fullVision = true;
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (ec.distanceToPlayer < ec.maxDistanceFromPlayer) 
        {
            if (time < timeToWait)
            {
                time += Time.deltaTime;

                positionOnBattleCircle = ec.modifiedPlayerPosition + ec.GetConnectedDirection() * ec.battleRadius;
                positionOnBattleCircle.y = ec.modifiedPosition.y;
                ec.SetNavMeshAgent(positionOnBattleCircle, ec.runSpeed, true);
            }
            else
            {
                time = 0f;

                if (Random.value > ec.actionChance) AIBehavior.ChangeState(AIBehavior.changePositionState);
                else AIBehavior.ChangeState(AIBehavior.attackState);
            }
        }
        else AIBehavior.ChangeState(AIBehavior.chaseState);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        time = 0f;

        ec.fullVision = false;
    }
}

public class ChangePositionState : State
{
    private Vector3 positionOnBattleCircle, curDirection, desirableDirection, desirablePos;

    private EnemyController ec;
    private EnemyAIBehavior AIBehavior;

    public ChangePositionState(EnemyAIBehavior AIBehavior) : base("Change Position State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        ec.fullVision = true;

        curDirection = ec.GetConnectedDirection();
        desirableDirection = ec.GetRandomDirection();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (ec.distanceToPlayer < ec.maxDistanceFromPlayer)
        {
            desirablePos = ec.modifiedPlayerPosition + desirableDirection * ec.battleRadius;
            if (Vector3.Distance(ec.modifiedPosition, desirablePos) <= ec.minDistance) AIBehavior.ChangeState(AIBehavior.battleState);
            else
            {
                curDirection = Vector3.MoveTowards(curDirection, desirableDirection, Time.deltaTime * ec.pointMoveSpeed).normalized;

                positionOnBattleCircle = ec.modifiedPlayerPosition + curDirection * ec.battleRadius;
                positionOnBattleCircle.y = ec.modifiedPosition.y;

                ec.SetNavMeshAgent(positionOnBattleCircle,
                    ec.distanceToPlayer < ec.battleRadius ? ec.walkSpeed : ec.runSpeed,
                    true);
            }
        }
        else AIBehavior.ChangeState(AIBehavior.chaseState);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        ec.fullVision = false;
    }
}

public class AttackState : State
{
    private Vector3 posOnCircle;

    private bool attacked;

    private EnemyController ec;
    private EnemyAIBehavior AIBehavior;
    
    public AttackState(EnemyAIBehavior AIBehavior) : base("Attack State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }
    
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        
        attacked = false;

        ec.navMeshAgent.stoppingDistance = ec.minDistanceToAttack;
    }
    
    public override void StateUpdate()
    {
        base.StateUpdate();

        if (ec.distanceToPlayer < ec.maxDistanceFromPlayer)
        {
            if (!attacked)
            {
                if (ec.distanceToPlayer > ec.minDistanceToAttack) ec.SetNavMeshAgent(ec.modifiedPlayerPosition, ec.runSpeed, true);
                else
                {
                    attacked = true;

                    ec.MakeOneAttack();

                    ec.UpdateBattleRadius();
                }
            }
            else
            {
                posOnCircle = ec.modifiedPlayerPosition + ec.GetConnectedDirection() * ec.battleRadius;
                posOnCircle.y = ec.modifiedPosition.y;

                ec.SetNavMeshAgent(posOnCircle, ec.runSpeed, true);

                if (Vector3.Distance(ec.modifiedPosition, posOnCircle) <= ec.minDistance + ec.navMeshAgent.stoppingDistance)
                {
                    AIBehavior.ChangeState(AIBehavior.battleState);
                }
            }
        }
        else AIBehavior.ChangeState(AIBehavior.chaseState);
    }
    
    public override void OnStateExit()
    {
        base.OnStateExit();
        
        attacked = false;

        ec.navMeshAgent.stoppingDistance = 0f;
    }
}

public class EscapeState : State
{
    private EnemyController ec;
    private EnemyAIBehavior AIBehavior;

    public EscapeState(EnemyAIBehavior AIBehavior) : base("Escape State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}

/*

||||||||||||||||||||||||||||
|||Template for New State|||
||||||||||||||||||||||||||||

public class TemplateState : State
{
    private EnemyController ec;
    private AIBehavior AIBehavior;

    public TemplateState(AIBehavior AIBehavior) : base("Template State", AIBehavior)
    {
        this.AIBehavior = AIBehavior;
        ec = AIBehavior.enemyController;
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}

*/
