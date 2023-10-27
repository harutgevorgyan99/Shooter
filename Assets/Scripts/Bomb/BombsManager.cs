using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BombsManager : Singleton<BombsManager>
{
    public int neutralizeBombsCount;
    public int bombsCount;
    [HideInInspector]public Bomb currentBomb;
    [SerializeField] private List<Bomb> allBombInScene = new List<Bomb>();
    [SerializeField] Text neutralizedBombCount;
    private void Start()
    {
        GameActionManager.Instance.OnRestartGame.AddListener(()=>Reset());
    }
    private void Update()
    {
        if (currentBomb != null && !currentBomb.isBombNeutralized)
        {
            Debug.Log("start Cheking");
            if (currentBomb.bombNeutralize.CheckKeyholded() && !currentBomb.bombNeutralize.isNeutralizeting)
            {
                currentBomb.bombNeutralize.StartTimer();
            }
            else if(!currentBomb.bombNeutralize.CheckKeyholded())
            {
                currentBomb.bombNeutralize.StopTimer();
            }
        }
    }
    public void Reset()
    {
        foreach (var item in allBombInScene)
        {
            item.isBombNeutralized = false;
            item.ResetAnim();
        }
        neutralizeBombsCount = 0;
        ShowNeutralizedBombsCount();
    }
    public void ShowNeutralizedBombsCount()
    {
        neutralizedBombCount.text = $"{neutralizeBombsCount} / {bombsCount}";
    }
    public void FinishingGameIfAllBombsNeutralized()
    {
        ShowNeutralizedBombsCount();
        currentBomb.OnBombNeutralize?.Invoke();
        if (ChekingIsAllBombsNeutralized())
            GameActionManager.Instance.OnPlayerWin?.Invoke();
    }
    public bool ChekingIsAllBombsNeutralized()
    {
        return neutralizeBombsCount == bombsCount;       
    }
}
