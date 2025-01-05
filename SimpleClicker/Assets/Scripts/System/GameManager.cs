using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // 알림 팝업창
    public NotificationUI NotificationUI;

    // 게임 데이터
    [HideInInspector]
    public string GameId;
    public PlayerState playerState;

    // 씬 로더
    public SceneLoader sceneLoader;

    public enum GameStep
    {
        None,
        Playing,
        Clear
    }
    public GameStep step;

    private GameManager()
    {
        playerState = new PlayerState();
        step = GameStep.None;
    }

    public void LoadScene(string sceneName) => sceneLoader.LoadScene(sceneName);
}
