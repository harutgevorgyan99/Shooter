using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float timeToDestroy;
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
        if (collision.gameObject.GetComponentInParent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();
            enemyHealth.TakeDamage(weapon.damage);

            if(enemyHealth.health<=0 && enemyHealth.isDead == false)
            {
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(dir * weapon.enemyKickbackForce, ForceMode.Impulse);
                enemyHealth.isDead = true;
            }
        }
        if (collision.gameObject.GetComponent<Player>())
        {
            Player pl = collision.gameObject.GetComponent<Player>();
            pl.TakeDamage(weapon.damage);

            if (pl.health <= 0 && pl.isDead == false)
            {
                pl.isDead = true;
            }
        }
        ObjectPooling.Instance.onObjectReachDestination?.Invoke();
    }
}
