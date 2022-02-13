using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void GameOver()
    {
        Time.timeScale = 0;
    }
}