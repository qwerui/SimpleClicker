using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private Text text;

    private void OnEnable()
    {
        playerState = GameManager.Instance.playerState;
        playerState.OnGoldChange += UpdateText;
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = Formatter.ShortenInteger(playerState.Gold);
    }

    private void OnDisable()
    {
        playerState.OnGoldChange -= UpdateText;
    }
}
