using UnityEngine;
using UnityEngine.UI;

public class DownloadPanel : MonoBehaviour
{
    public Text dataSizeText;
    public GameInitializer gameInitializer;

    public void SetDataSizeText(long size)
    {
        dataSizeText.text = $"{Formatter.ShortenIntegerByte(size)} 다운로드";
    }

    public void StopDownload() => gameInitializer.StopPipeline();
    public void StartDownload() => gameInitializer.StartDownload();
}
