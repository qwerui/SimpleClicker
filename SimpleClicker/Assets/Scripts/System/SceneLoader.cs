using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // 씬 로드
    [SerializeField] private Image loadingPanel;
    [HideInInspector]
    public bool sceneInitialized = true;
    bool isLoadingFaded;

    public void LoadScene(string sceneName)
    {
        isLoadingFaded = false;
        sceneInitialized = false;
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.DOFade(1, 1).OnComplete(() => { isLoadingFaded = true; });
        StartCoroutine(LoadSceneInternal(sceneName));
    }

    private IEnumerator LoadSceneInternal(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            if (isLoadingFaded)
            {
                op.allowSceneActivation = true;
            }
        }

        while (sceneInitialized)
        {
            yield return null;
        }

        loadingPanel.gameObject.SetActive(false);
        loadingPanel.color = Color.clear;
    }
}
