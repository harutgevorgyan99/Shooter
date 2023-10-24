using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingState : EnemyStates
{
    public override void EnterState(Enemy enemy)  => enemy.anim.SetBool("Walking", true);
    public override void UpdateState(Enemy enemy)
    {
        Debug.Log("Patroling");
        enemy.currentMoveSpeed = enemy.walkSpeed;
        if (!enemy.walkPointSet) enemy.SearchWalkPoint();
        enemy.agent.speed = enemy.walkSpeed;
        if (enemy.walkPointSet)
            enemy.agent.SetDestination(enemy.walkPoint);

        Vector3 distanceToWalkPoint = enemy.transform.position - enemy.walkPoint;
        enemy.anim.SetFloat("hzInput", 1);
        enemy.anim.SetFloat("vInput", 1);
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            enemy.walkPointSet = false;
    }
    public override void ExitState(Enemy enemy) => enemy.anim.SetBool("Walking", false);

}
