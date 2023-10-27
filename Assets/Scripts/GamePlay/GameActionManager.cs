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
    public UnityEvent OnPauseGame;
    public UnityEvent OnReplayGame;
    public UnityEvent OnStartGame;

    private void Start()
    {
        OnStartGame?.Invoke();
    }
    public void RestartGame()
    {
        OnRestartGame?.Invoke();
    }
    public void PauseGame()
    {
        OnPauseGame?.Invoke();
        Time.timeScale = 0;
    }
    public void RePlayGame()
    {
        OnReplayGame?.Invoke();
        Time.timeScale = 1;
    }
}
