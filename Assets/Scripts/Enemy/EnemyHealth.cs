using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    RagdollManager ragdollManager;
    [HideInInspector] public bool isDead;
    [SerializeField]Enemy enemy;

    private void Start()
    {
        ragdollManager = GetComponent<RagdollManager>();
    }

    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0) EnemyDeath();
            else Debug.Log("Hit");
        }
    }

    void EnemyDeath()
    {
        enemy.DeteacheListeners();
        enemy.enabled = false;
        enemy.agent.enabled = false;
       
        enemy.anim.enabled = false;
        ragdollManager.TriggerRagdoll();
        Debug.Log("Death");
    }
}
