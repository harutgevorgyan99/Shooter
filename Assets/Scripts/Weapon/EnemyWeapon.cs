using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    Enemy weaponOwner;
    void Start()
    {
        weaponOwner = GetComponentInParent<Enemy>();
        if (weaponOwner != null)
            weaponOwner.weapon = this;
        
        muzzleFlashLight = GetComponentInChildren<Light>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;
    }

    private void OnEnable()
    {
        if (weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponManager>();
            ammo = GetComponent<WeaponAmmo>();
            audioSource = GetComponent<AudioSource>();
           
        }

    }
    public void Update()
    {
        if (ShouldFire())
            Fire();
    }
    public override bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (ammo.currentAmmo == 0) {
            if (ammo.extraAmmo > 0)
                ammo.Reload();
            return false; 
        }
        if (weaponOwner.currentStates == weaponOwner.attacking)
            return true;
        return false;
    }

    public override void Fire()
    {
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
        fireRateTimer = 0;
        ammo.currentAmmo--;

        barrelPos.LookAt(weaponOwner.player);

        audioSource.PlayOneShot(gunShot);
        TriggerMuzzleFlash();
 

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Bullet currentBullet = ObjectPooling.Instance.GetObjectFromStorage(bullet);

            currentBullet.Init(barrelPos.position, barrelPos.rotation, this);
            currentBullet.dir = barrelPos.transform.forward;
            currentBullet.gameObject.SetActive(true);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
