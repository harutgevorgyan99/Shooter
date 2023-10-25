using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyStates
{
    public override void EnterState(Enemy enemy) => enemy.anim.SetBool("Running", true);

    public override void ExitState(Enemy enemy) => enemy.anim.SetBool("Running", false);

    public override void UpdateState(Enemy enemy)
    {
        
        enemy.anim.SetFloat("hzInput", 1);
        enemy.anim.SetFloat("vInput", 1);
        enemy.agent.speed = enemy.runSpeed;
        enemy.transform.LookAt(enemy.playerTransform);
        enemy.transform.localEulerAngles -= new Vector3(0, 45, 0); //45 is offset
        enemy.agent.SetDestination(enemy.playerTransform.position );
       // enemy.transform.LookAt(enemy.player);
    }
}
