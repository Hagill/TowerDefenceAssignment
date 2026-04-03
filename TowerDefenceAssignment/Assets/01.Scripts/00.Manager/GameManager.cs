using System;
using System.Collections;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public bool isPaused { get; private set; }
    public bool isGameOver { get; private set; }

    public void GameStartWithWaiting()
    {
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
    }

    public void GamePause()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void GameContinue()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void GameExit()
    {
        if (isGameOver)
        {
            isGameOver = false;
        }
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
    }
}
