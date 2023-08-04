using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    #region Health
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    private int health;
    #endregion

    #region AI Behavior
    [Header("AI Behavior")]
    [SerializeField, Tooltip("Minimum Distance from Player to Look at Them.")] private float minLookDistPlayer = 4f;
    [SerializeField, Tooltip("Minimum Distance from Player to Attack Them.")] private float minAttackDistPlayer = 1.5f;
    private float distFromPlayer;
    private Quaternion targetRotation;
    [SerializeField] private float rotationToPlayerSpeed = 2f;
    private bool isAttacking, isBlocking;
    [SerializeField] private float minTimeBetweenAttacks = 0.5f, maxTimeBetweenAttacks = 5f;
    [SerializeField] private float minTimeBlock = 2f, maxTimeBlock = 5f, blockDelay = 0.1f;
    [SerializeField, Range(0f, 1f)] private float blockChance = 0.75f;
    #endregion

    #region References
    [Header("References")]
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private Transform popUpTextSpawnPosition;
    private Animator anim;
    private GameObject player;
    private PopUpTextManager popUpTextManager;
    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
        popUpTextManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<PopUpTextManager>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        AIBehavior();
        Animation();

        if (health <= 0f) Death();
    }

    private void AIBehavior()
    {
        if (player != null) distFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distFromPlayer <= minLookDistPlayer && player != null)
        {
            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        }
        else isBlocking = false;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationToPlayerSpeed * Time.deltaTime);

        if (distFromPlayer <= minAttackDistPlayer && !isBlocking && player != null)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(AttackCycle());
            }
        }
        else isAttacking = false;
    }

    public void TakeDamage(int damage, bool willDodge)
    {
        anim.SetTrigger("Hit");

        if (!isBlocking || isBlocking && !willDodge)
        {
            health -= damage;
            popUpTextManager.DisplayDamagePopUpText("-" + damage.ToString(), popUpTextSpawnPosition);
            if (Random.value >= blockChance) StartCoroutine(BlockCycle());
        }
        else popUpTextManager.DisplayDodgedPopUpText(popUpTextSpawnPosition);
    }

    private void Death()
    {
        Instantiate(ragdollPrefab, transform.position, transform.rotation, transform.parent);
        popUpTextManager.DisplayDeathPopUpText(popUpTextSpawnPosition);
        player.GetComponent<PlayerController>().ResetFocus();
        Destroy(gameObject);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
    }

    private IEnumerator AttackCycle()
    {
        while (isAttacking)
        {
            Attack();
            yield return new WaitForSeconds(Random.Range(minTimeBetweenAttacks, maxTimeBetweenAttacks));
        }
    }

    private IEnumerator BlockCycle()
    {
        yield return new WaitForSeconds(blockDelay);
        isBlocking = true;
        yield return new WaitForSeconds(Random.Range(minTimeBlock, maxTimeBlock));
        isBlocking = false;
    }

    private void Animation()
    {
        anim.SetBool("Is Blocking", isBlocking);
    }
}
