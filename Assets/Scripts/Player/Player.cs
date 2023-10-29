using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] Animator anim;
    private Vector3 startPose;
    public int health;
    public float money=0;
    [HideInInspector] public int currentHelath;
    [HideInInspector] public bool isDead;
    [SerializeField] private RagdollManager ragdollManager;
    #region PlayerRelatedScripts
    [SerializeField] private MovementStateManager movementState;
    [SerializeField] private AimStateManager aimState; 
    [SerializeField] private ActionStateManager actionStateManager;
    [SerializeField] private Text helthInUI;
    [SerializeField] private Text moneyInUi;
     public WeaponManager weaponManager;
    #endregion
    [HideInInspector] public UnityEvent OnRespown;
    
    private void Start()
    {
        currentHelath = health;
        startPose = transform.position;
      
        GameActionManager.Instance.OnPlayerDead.AddListener(PlayerDeath);
        GameActionManager.Instance.OnRestartGame.AddListener(RespawnPlayer);
        GameActionManager.Instance.OnPauseGame.AddListener(PauseGame);
        GameActionManager.Instance.OnReplayGame.AddListener(ReplayGame);
        GameActionManager.Instance.OnRestartGame.AddListener(ShowHelthInUI);
        GameActionManager.Instance.OnStartGame.AddListener(ShowHelthInUI);
        GameActionManager.Instance.OnStartGame.AddListener(ShowPlayerMoneyInUI);
        GameActionManager.Instance.OnRestartGame.AddListener(ShowPlayerMoneyInUI);
    }
    public void TakeDamage(int damage)
    {
        if (currentHelath > 0)
        {
            currentHelath -= damage;
            if (currentHelath <= 0) GameActionManager.Instance.OnPlayerDead?.Invoke();
            else Debug.Log("Hit");
            ShowHelthInUI();
        }
    }
    public void PauseGame()
    {
        ChangePlayerRelatedComponentsStatus(false);
    }
    public void ReplayGame()
    {
        ChangePlayerRelatedComponentsStatus(true);
    }
    public void RespawnPlayer()
    {
       
       
        isDead = false;
        currentHelath = health;
        transform.position=startPose;
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
    public void ShowHelthInUI()
    {
        helthInUI.text = currentHelath.ToString();
    }
    public void ShowPlayerMoneyInUI()
    {
        moneyInUi.text = money.ToString();
    }
}
