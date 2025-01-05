using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;

public class GameRecorder : MonoBehaviour
{
    PlayerState playerState;
    WaitForSeconds interval;
    Dictionary<string, string> header = new();
    GameDataDTO gameDataDTO = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!AuthenticationService.Instance.IsAuthorized)
        {
            return;
        }
        header["Content-Type"] = "application/json";
        playerState = GameManager.Instance.playerState;
        interval = new WaitForSeconds(1.0f);
        StartCoroutine(UpdateGameDateToServer());
    }

    // 1초 마다 업로드
    IEnumerator UpdateGameDateToServer()
    {
        while(GameManager.Instance.step != GameManager.GameStep.Playing)
        {
            // 시작 할 때 까지 대기
            yield return null;
        }

        while(GameManager.Instance.step == GameManager.GameStep.Playing)
        {
            Debug.Log("Updating...");

            header["Authorization"] = GameSession.Instance.AccessToken;

            gameDataDTO.totalGold = playerState.TotalGold;
            gameDataDTO.gold = playerState.Gold;
            gameDataDTO.clickCount = playerState.ClickCount;
            gameDataDTO.killCount = playerState.EnemyKillCount;
            gameDataDTO.userId = AuthenticationService.Instance.PlayerId;

            string data = JsonUtility.ToJson(gameDataDTO);

            yield return NetworkManager.Instance.Put("http://localhost:5062/api/game/update",data,header);
            yield return NetworkManager.Instance.Put($"http://localhost:5062/api/user/connect/{AuthenticationService.Instance.PlayerId}", "", header);

            yield return interval;
        }
    }
}
