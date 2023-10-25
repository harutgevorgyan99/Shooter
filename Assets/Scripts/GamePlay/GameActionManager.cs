using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameActionManager : Singleton<GameActionManager>
{
    public Player player;
    public UnityEvent OnPlayerDead;
    public UnityEvent OnRestartGame;
    public UnityEvent OnPlayerWin;
    public void RestartGame()
    {
        OnRestartGame?.Invoke();
    }
}
