using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    private Player player;
    [SerializeField] private List<ItemForSale> allItems = new List<ItemForSale>();
    private void Start()
    {
        player = GameActionManager.Instance.player;
    }
    public void OpenShop()
    {
        UpdateBuyButtonStatus();
    }
    private void UpdateBuyButtonStatus()
    {
        foreach (var item in allItems)
        {
            bool status = CanBuyItem(item.price);
            item.btn.enabled = status;
            if (status)
            {
                item.btn.GetComponent<RawImage>().color = Color.white;
            }
            else
            {
                item.btn.GetComponent<RawImage>().color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    private bool CanBuyItem(int itemPrice)
    {
        return player.money >= itemPrice;
    }
    public void BuyWeapon(WeaponForSale weapon)
    {
        player.money -= weapon.price;
        player.weaponManager.SetCurrentWeapon(weapon.weapon);
        player.ShowPlayerMoneyInUI();
        UpdateBuyButtonStatus();
    }
    public void BuyBullets(BulletForSale bullet)
    {
        player.money -= bullet.price;
        player.weaponManager.currentWeapon.ammo.extraAmmo+=bullet.bulletCount;
        player.ShowPlayerMoneyInUI();
        UpdateBuyButtonStatus();
    }
}
