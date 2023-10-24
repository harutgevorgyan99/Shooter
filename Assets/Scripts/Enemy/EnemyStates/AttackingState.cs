using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : EnemyStates
{
    public override void EnterState(Enemy enemy) => enemy.anim.SetBool("Aiming", true);

    public override void ExitState(Enemy enemy) => enemy.anim.SetBool("Aiming", false);

    public override void UpdateState(Enemy enemy)
    {
        //Make sure enemy doesn't move
        enemy.agent.SetDestination(enemy.transform.position);

        enemy.transform.LookAt(enemy.player);
        enemy.transform.localEulerAngles += new Vector3(0, 45, 0);
        enemy.anim.SetFloat("hzInput", 0f);
        enemy.anim.SetFloat("vInput", 0f);
        enemy.agent.speed = 0;
        if (!enemy.alreadyAttacked)
        {
            ///Attack code here
            Debug.Log("detected");
            ///End of attack code

            enemy.alreadyAttacked = true;
            enemy.Invoke(nameof(enemy.ResetAttack), enemy.timeBetweenAttacks);
        }
    }
}
