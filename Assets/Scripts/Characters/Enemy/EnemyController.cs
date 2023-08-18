using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    #region Health
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int health;
    #endregion

    #region AI Behavior
    [Header("AI Behavior")]
    [SerializeField] private float sightRange = 25f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField, Range(0f, 360f)] private float viewAngle = 160f;
    [SerializeField] private Vector3 offsetFromObjectCenter = new Vector3(0f, 1f, 0f);
    [SerializeField] private float minPatrolRange = 5f, maxPatrolRange = 10f;
    [SerializeField] private float minDistanceToStop = 1f;
    [SerializeField, Range(0f, 0.1f)] private float minVelocityToStop = 0.1f;
    [SerializeField, Range(0.1f, 1f)] private float destinationUpdateDelay = 0.4f;
    [SerializeField] private float minTimeIdle = 10f, maxTimeIdle = 20f;
    private Vector3 modifiedPlayerPosition, modifiedPosition, destinationPosition;
    private Vector3 directionToPlayer;
    private float distanceToPlayer;
    private bool playerInSightRange, playerInAttackRange;
    private EnemyState curState;
    private bool inIdleState, patrolPointSet;
    private float movePower, desirableMovePower;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    #endregion

    #region Rotation
    [Header("Rotation")]
    [SerializeField, Range(0f, 1f)] private float rotationSpeed = 0.1f;
    private Vector3 lookPos;
    private Quaternion desiredRotation;
    #endregion

    #region Attack
    [Header("Attack")]
    [SerializeField] private float minTimeBetweenAttacks = 0.5f;
    [SerializeField] private float maxTimeBetweenAttacks = 5f;
    private bool isAttacking;
    #endregion

    #region Block
    [Header("Block")]
    [SerializeField] private float minTimeBlock = 2f;
    [SerializeField] private float maxTimeBlock = 5f;
    [SerializeField, Range(0f, 1f)] private float blockChance = 0.75f;
    private bool isBlocking;
    private bool blockedSuccessfully;
    #endregion

    #region Stuff
    [Header("Stuff")]
    [SerializeField, Tooltip("" +
        "Offset from object's center where pop-up text spawns." +
        "")] private Vector3 popUpTextOffset = new Vector3(0f, 1.25f, 0f);
    [SerializeField] private float animationTransitionDuration = 0.2f;
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private LayerMask obstacleLayerMask;
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private Animator anim;
    private PopUpTextManager popUpTextManager;
    #endregion

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
        GameObject mainManager = GameObject.FindGameObjectWithTag("Manager");
        popUpTextManager = mainManager.GetComponent<PopUpTextManager>();
    }

    private void Start()
    {
        health = maxHealth;

        destinationPosition = transform.position + offsetFromObjectCenter;
        curState = EnemyState.Idle;

        StartCoroutine(AIRoutine());
    }

    private void Update()
    {
        if (health <= 0f) Death();

        modifiedPlayerPosition = player.transform.position + offsetFromObjectCenter;
        modifiedPosition = transform.position + offsetFromObjectCenter;

        AIBehavior();
        AttackUpdate();
        Rotation();
        Animation();

        Debug.DrawLine(modifiedPosition, destinationPosition, Color.blue);
    }

    private void FixedUpdate() { CheckPlayerInRanges(); }

    public void TakeDamage(int damage, bool willDodge)
    {
        anim.SetTrigger("Hit");
        if (!isBlocking || isBlocking && !willDodge && !blockedSuccessfully)
        {
            health -= damage;
            popUpTextManager.DisplayDamagePopUpText(damage, transform.position + popUpTextOffset);
            if (Random.value >= blockChance) StartCoroutine(BlockCycle());
        }
        else popUpTextManager.DisplayDodgedPopUpText(transform.position + popUpTextOffset);
    }

    private void Death()
    {
        Instantiate(ragdollPrefab, transform.position, transform.rotation, transform.parent);
        popUpTextManager.DisplayDeathPopUpText(transform.position + popUpTextOffset);
        player.GetComponent<PlayerController>().DeleteFromFocusTargets(transform);
        Destroy(gameObject);
    }

    private void CheckPlayerInRanges()
    {
        if (player != null)
        {
            directionToPlayer = (modifiedPlayerPosition - modifiedPosition).normalized;
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                distanceToPlayer = Vector3.Distance(modifiedPosition, modifiedPlayerPosition);
                if (!Physics.Raycast(modifiedPosition, directionToPlayer, distanceToPlayer, obstacleLayerMask))
                {
                    if (distanceToPlayer <= sightRange) playerInSightRange = true;
                    else playerInSightRange = false;

                    if (distanceToPlayer <= attackRange) playerInAttackRange = true;
                    else playerInAttackRange = false;
                }
                else { playerInSightRange = false; playerInAttackRange = false; }
            }
            else { playerInSightRange = false; playerInAttackRange = false; }
        }
        else { playerInSightRange = false; playerInAttackRange = false; }
    }

    #region AI Behavior
    private IEnumerator AIRoutine()
    {
        while (true)
        {
            navMeshAgent.SetDestination(destinationPosition);
            yield return new WaitForSeconds(destinationUpdateDelay);
        }
    }

    private void AIBehavior()
    {
        if (!playerInSightRange)
        {
            if (curState == EnemyState.Patrol) PatrolState();
            else if (curState == EnemyState.Idle && !inIdleState) StartCoroutine(IdleState());
        }
    }

    private IEnumerator IdleState()
    {
        inIdleState = true;
        desirableMovePower = 0f;
        Debug.Log("Idle State.");
        yield return new WaitForSeconds(Random.Range(minTimeIdle, maxTimeIdle));
        curState = EnemyState.Patrol;
        inIdleState = false;
    }
    
    private void PatrolState()
    {
        if (!patrolPointSet)
        {
            Vector3 patrolPointPosition = GetPatrolPointPosition();

            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(patrolPointPosition, out myNavHit, maxPatrolRange, -1))
            {
                patrolPointPosition = myNavHit.position;
                patrolPointPosition = new Vector3(patrolPointPosition.x, patrolPointPosition.y + offsetFromObjectCenter.y, patrolPointPosition.z);

                patrolPointSet = true;
                desirableMovePower = 0.5f;
                destinationPosition = patrolPointPosition;
            }
        }

        if (patrolPointSet)
        {
            if (Vector3.Distance(modifiedPosition, destinationPosition) <= minDistanceToStop || navMeshAgent.velocity.magnitude <= minVelocityToStop)
            {
                patrolPointSet = false;
                curState = EnemyState.Idle;
            }
        }
    }

    private Vector3 GetPatrolPointPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        return modifiedPosition + randomDirection * Random.Range(minPatrolRange, maxPatrolRange);
    }
    #endregion

    private void Rotation()
    {
        if (playerInSightRange)
        {
            lookPos = modifiedPlayerPosition - modifiedPosition;
            lookPos.y = modifiedPosition.y;

            desiredRotation = Quaternion.LookRotation(lookPos);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed);
        }
    }

    private void AttackUpdate()
    {
        if (playerInAttackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackCycle());
            }
        }
        else isAttacking = false;
    }

    public void StartImmediateAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCycle());
        }
    }

    public void StopImmediateAttack() { isAttacking = false; }

    private IEnumerator AttackCycle()
    {
        while (isAttacking)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks));
        }
    }

    private IEnumerator BlockCycle()
    {
        isBlocking = true;
        yield return new WaitForSeconds(Random.Range(minTimeBlock, maxTimeBlock));
        isBlocking = false;
    }

    private void Animation()
    {
        if(movePower != desirableMovePower) movePower = Mathf.MoveTowards(movePower, desirableMovePower, animationTransitionDuration);
        anim.SetFloat("Move Power", Mathf.Clamp01(movePower));
        anim.SetBool("Is Blocking", isBlocking);
    }
}
