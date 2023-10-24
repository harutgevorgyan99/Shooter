using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator anim;
    public float walkSpeed = 3;
    public float runSpeed = 7;
    [HideInInspector] public float currentMoveSpeed=1;
    [HideInInspector] public float hzInput;
    [HideInInspector] public float vInput;
    #region states

    private AttackingState attacking = new AttackingState();
    private ChaseState chaseState = new ChaseState();
    private PatrolingState patrolingState = new PatrolingState();
    private EnemyStates currentStates;
    //Patroling
    [HideInInspector]public Vector3 walkPoint;
    [HideInInspector]public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    [HideInInspector] public bool alreadyAttacked;
   

    //States
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    #endregion

    private void Start()
    {
        player = GameActionManager.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        EnemyManager.Instance.ChekingPlayerPosition.AddListener(ChekingPlayerPosition);
        SwitchState(patrolingState);
    }
    public void SwitchState(EnemyStates  enemyState)
    {
        if (currentStates == enemyState)
            return;
        if (currentStates != null)
            currentStates.ExitState(this);
        currentStates = enemyState;
        currentStates.EnterState(this);
    }
    private void ChekingPlayerPosition()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) /*Patroling();//*/ SwitchState(patrolingState);
        if (playerInSightRange && !playerInAttackRange) SwitchState(chaseState);// ChasePlayer();
        if (playerInAttackRange && playerInSightRange) SwitchState(attacking);// AttackPlayer();
        currentStates.UpdateState(this);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    public void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        transform.LookAt(player);
        agent.SetDestination(player.position*currentMoveSpeed);
    }

    public void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Debug.Log("detected");
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void DeteacheListeners()
    {
        EnemyManager.Instance.ChekingPlayerPosition.RemoveListener(ChekingPlayerPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
