using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
   
    public Transform recoilFollowPos;
    ActionStateManager actions;
    public Weapon currentWeapon;
    public Weapon standartWeapon;
    [SerializeField] Transform weaponParent;
    [SerializeField] Transform weaponPlace;
    [SerializeField] Text bulletsCountinUI;
    private void Start()
    {
        if (actions == null) actions = GetComponent<ActionStateManager>();
        actions.SetWeapon(currentWeapon);
        GameActionManager.Instance.OnRestartGame.AddListener(Reset);
        GameActionManager.Instance.OnRestartGame.AddListener(ShowPlayerBulletsCountInUI);
        GameActionManager.Instance.OnStartGame.AddListener(ShowPlayerBulletsCountInUI);
    }
    public void SetCurrentWeapon(Weapon weapon)
    {
        if (weapon == currentWeapon)
            return;
        Destroy(currentWeapon.gameObject);
        currentWeapon = Instantiate(weapon);
        currentWeapon.transform.SetParent(weaponParent);
        currentWeapon.transform.localPosition = weaponPlace.localPosition;
        currentWeapon.transform.localRotation = weaponPlace.localRotation;
        currentWeapon.transform.localScale = weaponPlace.localScale;
        currentWeapon.gameObject.SetActive(true);
        actions.SetWeapon(currentWeapon);
    }
    public void ShowPlayerBulletsCountInUI()
    {
        bulletsCountinUI.text = $"{currentWeapon.ammo.currentAmmo}/{currentWeapon.ammo.extraAmmo}"; 
    }
    public void Reset()
    {
        SetCurrentWeapon(standartWeapon);
    }
}
