using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // 씬 로드
    [SerializeField] private Image loadingPanel;
    [HideInInspector]
    public bool sceneInitialized = true;
    bool isLoadingFaded;

    public void LoadScene(string sceneName, bool isAddressable)
    {
        isLoadingFaded = false;
        sceneInitialized = false;
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.DOFade(1, 1).OnComplete(() => { isLoadingFaded = true; });
        if(isAddressable)
        {
            StartCoroutine(LoadSceneAddressable(sceneName));
        }
        else
        {
            StartCoroutine(LoadSceneInternal(sceneName));
        }
        
    }

    private IEnumerator LoadSceneAddressable(string sceneName)
    {
        var op = Addressables.LoadSceneAsync(sceneName);

        while(!op.IsDone || !isLoadingFaded)
        {
            yield return null;

            if(isLoadingFaded && op.IsDone)
            {
                break;
            }
        }

        yield return WaitWhileInit();
        op.Result.ActivateAsync();
    }

    private IEnumerator LoadSceneInternal(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone || !isLoadingFaded)
        {
            yield return null;

            if (isLoadingFaded && op.isDone)
            {
                op.allowSceneActivation = true;
            }
        }

        yield return WaitWhileInit();
        
    }

    private IEnumerator WaitWhileInit()
    {
        while (sceneInitialized)
        {
            yield return null;
        }

        loadingPanel.gameObject.SetActive(false);
        loadingPanel.color = Color.clear;
    }
}
