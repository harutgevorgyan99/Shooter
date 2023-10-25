using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [HideInInspector] public Weapon weapon;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public int indexOfPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ObjectPooling.Instance.onObjectReachDestination.AddListener(() => SetObjcetsBackToPoolingObjectsCollection());
    }
    public void Init(Vector3 pos, Quaternion rot, Weapon weaponManager)
    {
        transform.position = pos;
        transform.rotation = rot;
        weapon = weaponManager;
    }
    public void SetObjcetsBackToPoolingObjectsCollection()
    {

        transform.position = ObjectPooling.Instance.startPosition.position;
        ObjectPooling.Instance.poolingObjects[indexOfPrefab].Enqueue(this);
        gameObject.SetActive(false);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Enemy>())
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            enemy.TakeDamage(weapon.damage);

            if(enemy.currentHelth<=0 && enemy.isDead == false)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(dir * weapon.enemyKickbackForce, ForceMode.Impulse);
                enemy.isDead = true;
            }
        }
       if (collision.gameObject.GetComponentInParent<Player>())
        {
            Player pl = collision.gameObject.GetComponentInParent<Player>();
            pl.TakeDamage(weapon.damage);

            if (pl.currentHelath <= 0 && pl.isDead == false)
            {
                pl.isDead = true;
            }
        }
        ObjectPooling.Instance.onObjectReachDestination?.Invoke();
    }
}
