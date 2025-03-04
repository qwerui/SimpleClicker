using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameInitializer : MonoBehaviour
{
    private bool downloadStart = false;

    public GameObject authPanel;
    public Text statusText;
    public DownloadPanel downloadPanel;

    public List<InventoryData> inventoryDatas = new();
    readonly Dictionary<int, InventoryData> inventoryPair = new();
    Dictionary<string, string> header = new Dictionary<string, string>();

    Coroutine pipeline;

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

    public void StartDownload() => downloadStart = true;

    public void OnClickPanel()
    {
        if(downloadStart)
        {
            return;
        }

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
        pipeline = StartCoroutine(InitializePipeline());
    }

    IEnumerator InitializePipeline()
    {
        
        yield return StartCoroutine(CheckUser());
        yield return StartCoroutine(DownloadAddressable());
        statusText.text = "Initializing...";
        yield return StartCoroutine(InitializeGameData());
        GameManager.Instance.LoadScene("Assets/Scenes/GameScene.unity", true);
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
        yield return Addressables.InitializeAsync(true);

        List<string> catalogsToUpdate = new List<string>();
        
        AsyncOperationHandle<List<string>> checkForUpdateHandle = Addressables.CheckForCatalogUpdates(true);
        checkForUpdateHandle.Completed += op =>
        {
            catalogsToUpdate.AddRange(op.Result);
        };
        yield return checkForUpdateHandle;

        List<IResourceLocator> locators = new List<IResourceLocator>();

        Debug.Log("Catalogs : " + catalogsToUpdate.Count);

        if (catalogsToUpdate.Count > 0)
        {
            AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(true, catalogsToUpdate, true);
            updateHandle.Completed += op =>
            {
                locators = op.Result;
            };
            yield return updateHandle;
        }
        else
        {
            locators.AddRange(Addressables.ResourceLocators);
        }

        List<object> keys = new List<object>();
        long downloadSize = 0;

        if (locators.Count > 0)
        {
            foreach (var locator in locators)
            {
                keys.AddRange(locator.Keys);
            }

            var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
            sizeHandle.Completed += op =>
            {
                downloadSize = op.Result;
                Debug.Log("Download Size : "+op.Result);
                Addressables.Release(sizeHandle);
            };
            yield return sizeHandle;
        }

        if (downloadSize > 0)
        {
            downloadPanel.SetDataSizeText(downloadSize);
            downloadPanel.gameObject.SetActive(true);

            while(true)
            {
                if(downloadStart)
                {
                    break;
                }
                yield return null;
            }

            AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.None, true);
            while (!downloadHandle.IsDone)
            {
                statusText.text = $"다운로드 중 : {downloadHandle.PercentComplete*100}%";
                yield return null;
            }
        }
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
                playerState.EnemyKillCount = dto.killCount;
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

    public void StopPipeline()
    {
        if(pipeline != null)
        {
            StopCoroutine(pipeline);
        }
    }

    private void OnDisable()
    {
        AuthenticationService.Instance.SignedIn -= StartGame;
    }

}
