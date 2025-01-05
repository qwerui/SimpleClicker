using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Services.Authentication;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public PlayerState playerState;
    public EnemyModel model;
    public SoundVolumeModel volumeModel;

    public InventoryData clickUpgrade;
    public InventoryData clearUpgrade; 
    public GameObject clearPanel;

    Dictionary<string, string> header = new Dictionary<string, string>();

    private void OnEnable()
    {
        playerState = GameManager.Instance.playerState;
        clickUpgrade.OnUpgrade += UpgradeClick;
        model.OnEnemyDied.Callback += AddKill;
        clearUpgrade.OnUpgrade += Clear;
        header["Content-Type"] = "application/json";
        UpgradeClick();
    }

    private IEnumerator Start()
    {
        volumeModel.mixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGM", 0f));
        volumeModel.mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX", 0f));

        header["Authorization"] = GameSession.Instance.AccessToken;

        if(AuthenticationService.Instance.IsAuthorized)
        {
            yield return NetworkManager.Instance.Get($"http://localhost:5062/api/user/connect?userId={AuthenticationService.Instance.PlayerId}", (result) =>
            {
                // 어째선지 큰 따옴표가 붙어온다.
                result = result.Replace("\"","");

                int timediff = (int)DateTimeOffset.UtcNow.Subtract(DateTimeOffset.Parse(result)).TotalMinutes;

                if (timediff >= 10)
                {
                    int restGold = (int)(timediff * 0.1f * Mathf.Max(GameManager.Instance.playerState.EnemyKillCount,1) + GameManager.Instance.playerState.ClickCount * 0.01f);
                    GameManager.Instance.playerState.Gold += restGold;
                    var notification = GameManager.Instance.NotificationUI;

                    notification.mainButtonText.text = "확인";
                    notification.description.text = $"휴식보상 {Formatter.ShortenInteger(restGold)} 골드 획득!";

                    notification.Activate(false);
                }

            }, header);

            yield return NetworkManager.Instance.Put($"http://localhost:5062/api/user/connect/{AuthenticationService.Instance.PlayerId}", "", header);
        }

        GameManager.Instance.step = GameManager.GameStep.Playing;
    }

    void AddKill() => playerState.EnemyKillCount++;
    void UpgradeClick() => playerState.ClickDamage = Mathf.Max(clickUpgrade.UpgradeCount * 3, 1);

    private void Clear()
    {
        clearPanel.SetActive(true);
        GameManager.Instance.step = GameManager.GameStep.Clear;
        StartCoroutine(UploadClear());
    }

    IEnumerator UploadClear()
    {
        var dto = new GameDataDTO
        {
            userId = AuthenticationService.Instance.PlayerId,
            killCount = playerState.EnemyKillCount,
            totalGold = playerState.TotalGold,
            clickCount = playerState.ClickCount
        };

        var data = JsonUtility.ToJson(dto);

        
        header["Authorization"] = GameSession.Instance.AccessToken;

        yield return NetworkManager.Instance.Put("http://localhost:5062/api/game/clear", data, header);
        yield return NetworkManager.Instance.Put($"http://localhost:5062/api/user/connect/{AuthenticationService.Instance.PlayerId}", "", header);
    }

    private void OnDisable()
    {
        model.OnEnemyDied.Callback -= AddKill;
    }
}
