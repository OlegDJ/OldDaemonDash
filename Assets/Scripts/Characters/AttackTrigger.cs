using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField, Tooltip("1 - player, 2 - enemy.")] private byte characterType;
    [SerializeField] private Transform leadBodyPart;
    [SerializeField] private int minDamage, maxDamage;
    [SerializeField] private bool canBeDodged;

    private void FixedUpdate()
    {
        transform.position = leadBodyPart.position;
        transform.rotation = leadBodyPart.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && characterType == 1)
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            if (enemyController != null) enemyController.TakeDamage(Random.Range(minDamage, maxDamage), canBeDodged);
        }
        else if (other.CompareTag("Player") && characterType == 2)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null) playerController.TakeDamage(Random.Range(minDamage, maxDamage), canBeDodged);
        }
    }
}
