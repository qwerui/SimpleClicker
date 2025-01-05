using UnityEngine;
using UnityEngine.UI;

public class PrefToSlider : MonoBehaviour
{
    public string prefKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(prefKey, 0f);
    }
}
