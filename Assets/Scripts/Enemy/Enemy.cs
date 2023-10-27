using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public int indexOfPrefab;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public Player player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Animator anim;
    public float walkSpeed = 3;
    public float runSpeed = 7;
    [HideInInspector] public float currentMoveSpeed=1;
    [HideInInspector] public float hzInput;
    [HideInInspector] public float vInput;
    [HideInInspector] public EnemyWeapon weapon;

    public RagdollManager ragdollManager;
    [HideInInspector] public bool isDead;
    #region EnemyInitialParams
    private Vector3 startPose;
    private float health;
    private int damage;
    [HideInInspector] public float currentHelth;
    #endregion
    #region states

    [HideInInspector] public AttackingState attacking = new AttackingState();
    private ChaseState chaseState = new ChaseState();
    private PatrolingState patrolingState = new PatrolingState();
    [HideInInspector] public EnemyStates currentStates;
    //Patroling
    [HideInInspector]public Vector3 walkPoint;
    [HideInInspector]public bool walkPointSet;
    public float walkPointRangeMax;
    public float walkPointRangeMin;
    //Attacking
    public float timeBetweenAttacks;
    [HideInInspector] public bool alreadyAttacked;
   

    //States
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    #endregion


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

        if (!playerInSightRange && !playerInAttackRange || player.isDead) /*Patroling();//*/ SwitchState(patrolingState);
        if (playerInSightRange && !playerInAttackRange && !player.isDead) SwitchState(chaseState);// ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !player.isDead) SwitchState(attacking);// AttackPlayer();
        currentStates.UpdateState(this);
    }
    public void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(walkPointRangeMin, walkPointRangeMax);
        float randomX = Random.Range(walkPointRangeMin, walkPointRangeMax);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    public void ResetAttack()
    {
        alreadyAttacked = false;
    }
    public void DeteacheListeners()
    {
        EnemyManager.Instance.ChekingPlayerPosition.RemoveListener(ChekingPlayerPosition);
    }
    public void TakeDamage(float damage)
    {
        if (currentHelth > 0)
        {
            currentHelth -= damage;
            if (currentHelth <= 0) EnemyDeath();
            else Debug.Log("Hit");
        }
    }

    void EnemyDeath()
    {
        DeteacheListeners();
        ChangeEnemyRelatedComponentsStatus(false);
        ragdollManager.TriggerRagdoll();
        SetObjcetsBackToPoolingObjectsCollection();
        EnemyManager.Instance.OnEnemyDead?.Invoke();
        Debug.Log("Death");
    }
    public void Init(Vector3 startPose, float health, int damage)
    {
        this.startPose = startPose;
        this.health = health;
        this.damage = damage;

        transform.position = startPose;
        currentHelth = health;
        weapon.damage = damage;
        player = GameActionManager.Instance.player;
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        EnemyManager.Instance.ChekingPlayerPosition.AddListener(ChekingPlayerPosition);
        SwitchState(patrolingState);
    }
    void ChangeEnemyRelatedComponentsStatus(bool status)
    {
        weapon.enabled = status;
        enabled = status;
        anim.enabled = status;
        agent.enabled = status;
    }
    public void SetObjcetsBackToPoolingObjectsCollection()
    {

        
        ObjectPooling.Instance.poolingObjects[indexOfPrefab].Enqueue(this);
        

    }
    public void Reset()
    {
        isDead = false;
        transform.position = startPose;
        currentHelth = health;
        weapon.damage = damage;
        ChangeEnemyRelatedComponentsStatus(true);
        EnemyManager.Instance.ChekingPlayerPosition.AddListener(ChekingPlayerPosition);
        ragdollManager.Reset();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
