using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyAIBehavior : StateMachine
{
    [HideInInspector] public EnemyController enemyController;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public BattleState battleState;
    [HideInInspector] public ChangePositionState changePositionState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public EscapeState escapeState;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();

        idleState = new IdleState(this);
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        battleState = new BattleState(this);
        changePositionState = new ChangePositionState(this);
        attackState = new AttackState(this);
        escapeState = new EscapeState(this);
    }

    protected override State GetInitialState() { return idleState; }
}
