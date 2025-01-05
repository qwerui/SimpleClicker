using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    public InventoryData data;

    [SerializeField] private Image icon;
    [SerializeField] private Text description;
    [SerializeField] private Text level;
    [SerializeField] private Text requireGold;
    [SerializeField] private GameObject lockPanel;

    private bool locked = true;
    Dictionary<string, string> header = new();

    public void Init()
    {
        data.OnUpgrade += OnUpgrade;
        icon.sprite = data.Icon;
        description.text = data.Description;
        requireGold.text = data.GetRequireGoldString();
        level.text = $"Level : {data.GetUpgradeCountString()}";
        header["Content-Type"] = "application/json";
        OnUpgrade();
    }

    private void OnEnable()
    {
        if(data != null)
        {
            Init();
        }
    }

    private void Update()
    {
        if(locked && !data.Locked)
        {
            locked = false;
            lockPanel.SetActive(false);
        }
    }

    public void Upgrade()
    {
        if (GameManager.Instance.playerState.Gold >= data.RequireGold)
        {
            GameManager.Instance.playerState.Gold -= data.RequireGold;
            data.UpgradeCount++;

            if (AuthenticationService.Instance.IsAuthorized)
            {
                StartCoroutine(UpdateUpgradeToServer());
            }
        }
    }

    private void OnUpgrade()
    {
        requireGold.text = data.GetRequireGoldString();
        level.text = $"Level : {data.GetUpgradeCountString()}";
    }

    private IEnumerator UpdateUpgradeToServer()
    {
        Debug.Log(GameManager.Instance.GameId);
        header["Authorization"] = GameSession.Instance.AccessToken;
        string json = JsonUtility.ToJson(new UpgradeDTO {gameId=GameManager.Instance.GameId, upgradeId=data.id, amount=data.UpgradeCount});
        yield return NetworkManager.Instance.Put("http://localhost:5062/api/game/upgrade", json, header);
    }

    private void OnDisable()
    {
        data.OnUpgrade -= OnUpgrade;
    }
}
