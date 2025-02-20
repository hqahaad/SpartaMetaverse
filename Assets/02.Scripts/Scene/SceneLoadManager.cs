using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEditor;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;

//addtive 씬으로 수정, 언로드 씬 추가
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeAmount = 5f;
    [SerializeField] private float fakeLoadingDelaySecond = 0.3f;

    private bool isLoaded = false;

    public event Action OnSceneLoaded = delegate { };
    public event Action OnSceneUnloaded = delegate { };

    public async void LoadScene(AssetReferenceT<SceneAsset> sceneReference, DisplayLoading displayLoading = DisplayLoading.On)
    {
        await LoadSceneAsync(sceneReference, displayLoading);
    }

    private async Task LoadSceneAsync(AssetReferenceT<SceneAsset> sceneReference, DisplayLoading displayLoading = DisplayLoading.On)
    {
        if (!sceneReference.RuntimeKeyIsValid())
        {
            isLoaded = false;
            return;
        }

        if (isLoaded)
        {
            return;
        }

        isLoaded = true;

        if (displayLoading == DisplayLoading.On)
        {
            LoadingEnable();
        }

        var handle = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Single, false);

        while (fadeImage.fillAmount < 1f)
        {
            await UniTask.NextFrame();

            fadeImage.fillAmount += fadeAmount * Time.unscaledDeltaTime;
        }

        fadeImage.fillAmount = 1f;

        await handle;

        //**await Unload**

        var currentScene = SceneManager.GetActiveScene();
        //var unload = Addressables.UnloadSceneAsync(currentScene.path.);


        //unload 

        OnSceneUnloaded?.Invoke();

        await handle.Result.ActivateAsync();

        OnSceneLoaded?.Invoke();

        await UniTask.Delay((int)fakeLoadingDelaySecond * 1000, ignoreTimeScale: true);

        while (fadeImage.fillAmount > 0.05f)
        {
            await UniTask.NextFrame();

            fadeImage.fillAmount -= fadeAmount * Time.unscaledDeltaTime;
        }

        fadeImage.fillAmount = 0f;

        LoadingDisable();

        isLoaded = false;
    }

    private void LoadingEnable()
    {
        myCanvas.gameObject.SetActive(true);
        fadeImage.fillAmount = 0f;
    }

    private void LoadingDisable()
    {
        myCanvas.gameObject.SetActive(false);
    }
}

public enum DisplayLoading
{
    On,
    Off
}
