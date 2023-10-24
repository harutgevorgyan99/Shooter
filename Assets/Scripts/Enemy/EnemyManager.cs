using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : Singleton<EnemyManager>
{
    public UnityEvent ChekingPlayerPosition;

    private void Update()
    {
        ChekingPlayerPosition?.Invoke();
    }
}
