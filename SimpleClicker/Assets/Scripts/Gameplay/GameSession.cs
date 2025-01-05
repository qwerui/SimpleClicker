using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class GameSession : Singleton<GameSession>
{
    NotificationUI notification;

    public string AccessToken { get => "Bearer "+AuthenticationService.Instance.AccessToken; }

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.Expired += NotifyReturnTitle;
    }

    private void Start()
    {
        notification = GameManager.Instance.NotificationUI;
    }

    public async void Signin()
    {
        try
        {
            // 익명, 플랫폼 로그인 관계없이 이 메소드가 세션 토큰을 바탕으로 유저 정보를 복구한다.
            // Access Token은 자동 Refresh
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError($"Sign in anonymously failed with error code: {ex.ErrorCode}");
        }
    }

    void NotifyReturnTitle()
    {
        notification.description.text = "세션이 만료되어 타이틀로 돌아갑니다.";
        notification.mainButtonText.text = "확인";
        notification.RegisterToMain(ReturnTitle);
        notification.Activate(false);
    }

    void ReturnTitle()
    {
        notification.UnregisterFromMain(ReturnTitle);
        GameManager.Instance.LoadScene("MainScene");
    }
}
