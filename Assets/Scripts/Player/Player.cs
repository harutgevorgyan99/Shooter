using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] Animator anim;
    private Vector3 startPose;
    public float health;
    [HideInInspector] public float currentHelath;
    [HideInInspector] public bool isDead;
    [SerializeField] private RagdollManager ragdollManager;
    #region PlayerRelatedScripts
    [SerializeField] private MovementStateManager movementState;
    [SerializeField] private AimStateManager aimState;
    [SerializeField] private ActionStateManager actionStateManager;
    [SerializeField] private WeaponManager weaponManager;
    #endregion
    public UnityEvent OnDead;
    public UnityEvent OnRespown;
    private void Start()
    {
        GameActionManager.Instance.OnPlayerDead.AddListener(PlayerDeath);
        GameActionManager.Instance.OnRestartGame.AddListener(RespawnPlayer);
        currentHelath=health;
        startPose = transform.position;
    }
    public void TakeDamage(float damage)
    {
        if (currentHelath > 0)
        {
            currentHelath -= damage;
            if (currentHelath <= 0) GameActionManager.Instance.OnPlayerDead?.Invoke();
            else Debug.Log("Hit");
        }
    }

    public void RespawnPlayer()
    {
        currentHelath = health;
        transform.position=startPose;
        isDead = false;
        ChangePlayerRelatedComponentsStatus(true);
        ragdollManager.Reset();
    }
    public void PlayerDeath()
    {
        ChangePlayerRelatedComponentsStatus(false);

        ragdollManager.TriggerRagdoll();
        Debug.Log("Death");
    }
    void ChangePlayerRelatedComponentsStatus(bool status)
    {
        weaponManager.enabled = status;
        weaponManager.currentWeapon.enabled = status;
        movementState.enabled = status;
        aimState.enabled = status;
        actionStateManager.enabled = status;
        anim.enabled = status;
    }
}
