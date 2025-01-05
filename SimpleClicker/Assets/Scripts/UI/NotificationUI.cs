using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    public Text description;

    public Button mainButton;
    public Text mainButtonText;

    public Button subButton;
    public Text subButtonText;

    public void RegisterToMain(UnityAction callback)
    {
        mainButton.onClick.AddListener(callback);
    }

    public void RegisterToSub(UnityAction callback)
    {
        subButton.onClick.AddListener(callback);
    }

    public void UnregisterFromMain(UnityAction callback)
    {
        mainButton.onClick.RemoveListener(callback);
    }

    public void UnregisterFromSub(UnityAction callback)
    {
        subButton.onClick.RemoveListener(callback);
    }

    public void Activate(bool activateSub)
    {
        gameObject.SetActive(true);
        subButton.gameObject.SetActive(activateSub);
    }
}
