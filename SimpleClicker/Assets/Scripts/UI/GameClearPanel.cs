using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameClearPanel : MonoBehaviour
{
    [Header("References")]
    public RectTransform PanelBackground;

    public Text clickCountText;
    public Text killCountText;
    public Text totalGoldText;

    [Header("Events")]
    public VoidEvent ClearEvent;

    bool resultEnd = false;

    private void OnEnable()
    {
        ClearEvent.Callback += Show;
    }

    private void Show()
    {
        gameObject.SetActive(true);

        clickCountText.text = $"클릭횟수 : {GameManager.Instance.playerState.ClickCount}";
        killCountText.text = $"처치횟수 : {GameManager.Instance.playerState.EnemyKillCount}";
        totalGoldText.text = $"누적골드 : {GameManager.Instance.playerState.TotalGold}";

        PanelBackground
            .DOAnchorPosY(0, 1.5f)
            .OnComplete(()=>resultEnd = true);
    }

    public void ReturnTitle()
    {
        if (resultEnd) 
        {
            resultEnd = false;
            GameManager.Instance.step = GameManager.GameStep.None;
            GameManager.Instance.LoadScene("MainScene");
        }
    }

    private void OnDisable()
    {
        ClearEvent.Callback -= Show;
    }
}
