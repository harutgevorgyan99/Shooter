using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public float health;
    [HideInInspector] public bool isDead;
    public void TakeDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0) PlayerDeath();
            else Debug.Log("Hit");
        }
    }

    void PlayerDeath()
    {
     
        Debug.Log("Death");
    }
}
