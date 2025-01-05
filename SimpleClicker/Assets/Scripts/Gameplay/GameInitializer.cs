using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    public GameObject authPanel;
    public Text statusText;

    public List<InventoryData> inventoryDatas = new();
    readonly Dictionary<int, InventoryData> inventoryPair = new();
    Dictionary<string, string> header = new Dictionary<string, string>();

    private void Awake()
    {
        foreach(var inventory in inventoryDatas)
        {
            inventoryPair[inventory.id] = inventory;
        }

        Debug.Assert(inventoryDatas.Count == inventoryPair.Count);
    }

    private void Start()
    { 
        AuthenticationService.Instance.SignedIn += StartGame;
    }

    public void OnClickPanel()
    {
        if(!AuthenticationService.Instance.SessionTokenExists)
        {
            authPanel.SetActive(true);
            return;
        }

        if(!AuthenticationService.Instance.IsAuthorized)
        {
            GameSession.Instance.Signin();
        }
        else
        {
            StartGame();
        }
        
    }

    public void OnClickGuestSignin()
    {
        GameSession.Instance.Signin();
        authPanel.SetActive(false);
    }

    void StartGame()
    {
        statusText.text = "Initializing...";
        StartCoroutine(InitializePipeline());
    }

    IEnumerator InitializePipeline()
    {
        //yield return StartCoroutine(DownloadAddressable());
        yield return StartCoroutine(CheckUser());
        yield return StartCoroutine(InitializeGameData());
        GameManager.Instance.LoadScene("GameScene");
    }

    private IEnumerator CheckUser()
    {
        bool exist = true;

        header["Authorization"] = GameSession.Instance.AccessToken;

        yield return NetworkManager.Instance.Get($"http://localhost:5062/api/user?userId={AuthenticationService.Instance.PlayerId}", (result) => { }, header, catchCallback: (error) =>
        {
            exist = false;
        });

        if(!exist)
        {
            WWWForm form = new WWWForm();
            form.AddField("userId", AuthenticationService.Instance.PlayerId);

            yield return NetworkManager.Instance.Post("http://localhost:5062/api/user", form, header);
        }
    }

    IEnumerator DownloadAddressable()
    {
        yield return null;
    }

    /// <summary>
    /// 서버 DB에서 게임 데이터 조회
    /// </summary>
    /// <returns></returns>
    IEnumerator InitializeGameData()
    {
        bool badReqeust = false;
        header["Authorization"] = GameSession.Instance.AccessToken;

        yield return NetworkManager.Instance.Get($"http://localhost:5062/api/game?userId={AuthenticationService.Instance.PlayerId}", 
            (result) => 
            {
                var dto = JsonUtility.FromJson<GameDataDTO>(result);

                var playerState = GameManager.Instance.playerState;
                playerState.ClickCount = dto.clickCount;
                playerState.TotalGold = dto.totalGold;
                playerState.Gold = dto.gold;
                playerState.ClickCount = dto.clickCount;
                
                GameManager.Instance.GameId = dto.gameId;

                if (dto.upgrades != null)
                {
                    
                    foreach (var upgrade in dto.upgrades)
                    {
                        inventoryPair[upgrade.upgradeId].UpgradeCount = upgrade.amount;
                    }
                }

            }, header,
            catchCallback: (error) => 
            {
                if (error.code == 400)
                {
                    badReqeust = true;
                }
            });

        // 진행 중인 데이터가 없다면 새 데이터를 생성
        if (badReqeust)
        {
            WWWForm form = new WWWForm();
            form.AddField("userId", AuthenticationService.Instance.PlayerId);

            yield return NetworkManager.Instance.Post("http://localhost:5062/api/game/init", form, header, callback: (result) => 
            {
                GameManager.Instance.GameId = result;
            });

            foreach(var inventory in inventoryDatas)
            {
                inventory.UpgradeCount = 0;
            }
        }
    }

    private void OnDisable()
    {
        AuthenticationService.Instance.SignedIn -= StartGame;
    }

}
