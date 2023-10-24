using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponManager : MonoBehaviour
{
   
    public Transform recoilFollowPos;
    ActionStateManager actions;
    public Weapon currentWeapon;
    [SerializeField] Transform weaponParent;
    [SerializeField] Transform weaponPlace;
    private void Start()
    {
        if (actions == null) actions = GetComponent<ActionStateManager>();
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
}
