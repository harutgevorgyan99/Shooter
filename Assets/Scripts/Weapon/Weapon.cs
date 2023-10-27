using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Fire Rate")]
    [SerializeField]protected float fireRate;
    [SerializeField] protected bool semiAuto;
    protected float fireRateTimer;

    [Header("Bullet Properties")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform barrelPos;
    [SerializeField] protected float bulletVelocity;
    [SerializeField] protected int bulletsPerShot;
    public int damage = 20;
    protected AimStateManager aim;

    [SerializeField] protected AudioClip gunShot;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public WeaponAmmo ammo;
     ActionStateManager actions;
    protected WeaponRecoil recoil;

    protected Light muzzleFlashLight;
    protected ParticleSystem muzzleFlashParticles;
    protected float lightIntensity;
    [SerializeField] protected float lightReturnSpeed=20;

    public float enemyKickbackForce = 100;
    protected WeaponManager weaponClass;

    // Start is called before the first frame update
    void Start()
    {
        
        aim = GetComponentInParent<AimStateManager>();
        
        actions = GetComponentInParent<ActionStateManager>();
        muzzleFlashLight = GetComponentInChildren<Light>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        fireRateTimer = fireRate;
    }

    private void OnEnable()
    {
        if(weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponManager>();
            ammo = GetComponent<WeaponAmmo>();
            audioSource = GetComponent<AudioSource>();
            recoil = GetComponent<WeaponRecoil>();
            recoil.recoilFollowPos = weaponClass.recoilFollowPos;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(ShouldFire()) Fire();
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
    }

    public virtual bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (ammo.currentAmmo == 0) return false;
        if (actions.currentState == actions.Reload) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    public virtual void Fire()
    {
        fireRateTimer = 0;
        ammo.currentAmmo--;

        barrelPos.LookAt(aim.aimPos);

        audioSource.PlayOneShot(gunShot);
        TriggerMuzzleFlash();
        recoil.TriggerRecoil();
        
        for(int i =0; i < bulletsPerShot; i++)
        {
            Bullet currentBullet = ObjectPooling.Instance.GetObjectFromStorage(bullet);

            currentBullet.Init(barrelPos.position, barrelPos.rotation, this);
            currentBullet.transform.forward = barrelPos.transform.forward;
            currentBullet.dir = barrelPos.transform.forward;
            currentBullet.gameObject.SetActive(true);
            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            rb.velocity=(barrelPos.forward * bulletVelocity);
        }
        weaponClass.ShowPlayerBulletsCountInUI();
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
