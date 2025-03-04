using UnityEngine;
using UnityEngine.UI;

public class CacheClear : MonoBehaviour
{
    public GameObject beforeButtons;
    public GameObject afterButtons;
    public Text cacheText;

    public void ClearCache()
    {
        bool isSuccess = Caching.ClearCache();
        beforeButtons.SetActive(false);

        if(isSuccess)
        {
            cacheText.text = "데이터가 삭제되었습니다.";
        }
        else
        {
            cacheText.text = "데이터 삭제 중 오류가 발생했습니다.";
        }

        afterButtons.SetActive(true);
    }
}
