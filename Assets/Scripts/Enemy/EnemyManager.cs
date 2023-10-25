using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : Singleton<EnemyManager>
{
    public UnityEvent ChekingPlayerPosition;
    [SerializeField] private GameObject enemePrefab;
    [SerializeField] private Transform enemysParent;
    [SerializeField] int enemyCountInScene;
    [SerializeField] float enemyRespawnTime;
    [SerializeField] float maxHealth, minHealth, maxDamage, minDmage;
    public List<Transform> possibleEnemyPlaces = new List<Transform>();
    private List<Enemy> enemysInScene = new List<Enemy>();
    public UnityEvent OnEnemyDead;
    private void Start()
    {
        ObjectPooling.Instance.CreatePoolingObjectsForEnemys(enemePrefab, enemysParent, enemyCountInScene);
        GameActionManager.Instance.OnRestartGame.AddListener(ResetEnemysInRestart);
        OnEnemyDead.AddListener(RespawnEnemy);
        for (int i = 0; i < enemyCountInScene; i++)
        {

           Enemy enemmy= ObjectPooling.Instance.GetEnemyFromStorage(enemePrefab);
           Vector3 startPose = (i < possibleEnemyPlaces.Count) ? possibleEnemyPlaces[i].position : possibleEnemyPlaces[Random.Range(0, possibleEnemyPlaces.Count - 1)].position;
           float damage = Random.Range(minDmage, maxDamage);
           float health = Random.Range(minHealth, maxHealth);
           enemmy.Init(startPose, health, damage);
           enemysInScene.Add(enemmy);
        }
       
    }
    public void ResetEnemysInRestart()
    {
        foreach (var item in enemysInScene)
        {
            item.Reset();
        }
    }
    void RespawnEnemy()
    {
        StartCoroutine(RespawningEnemy());
    }
    IEnumerator RespawningEnemy()
    {
        yield return new WaitForSeconds(enemyRespawnTime);
        Enemy enemmy = ObjectPooling.Instance.GetEnemyFromStorage(enemePrefab);
        enemmy.gameObject.SetActive(false);
        enemmy.Reset();
        enemmy.gameObject.SetActive(true);
    }
    private void Update()
    {
        ChekingPlayerPosition?.Invoke();
    }
}
