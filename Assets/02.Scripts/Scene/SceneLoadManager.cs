using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEditor;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

//addtive ¾À, ¾ð·Îµå ¾À Ãß°¡
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeAmount = 5f;
    [SerializeField] private float fakeLoadingDelaySecond = 0.3f;

    private bool isLoaded = false;

    public async void LoadScene(AssetReferenceT<SceneAsset> sceneReference, DisplayLoading displayLoading = DisplayLoading.Display)
    {
        await LoadSceneAsync(sceneReference, displayLoading);
    }

    private async Task LoadSceneAsync(AssetReferenceT<SceneAsset> sceneReference, DisplayLoading displayLoading = DisplayLoading.Display)
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

        await Task.Delay(10);

        if (displayLoading == DisplayLoading.Display)
        {
            LoadingEnable();
        }

        var handle = Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Single, false);

        while (fadeImage.fillAmount < 1f)
        {
            await UniTask.NextFrame();

            fadeImage.fillAmount += fadeAmount * Time.deltaTime;
        }

        fadeImage.fillAmount = 1f;

        await handle;

        await handle.Result.ActivateAsync();

        await UniTask.Delay((int)fakeLoadingDelaySecond * 1000);

        while (fadeImage.fillAmount > 0.05f)
        {
            await UniTask.NextFrame();

            fadeImage.fillAmount -= fadeAmount * Time.deltaTime;
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
    None,
    Display
}
