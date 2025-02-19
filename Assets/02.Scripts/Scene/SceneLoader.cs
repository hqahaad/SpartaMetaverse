using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private AssetReferenceT<SceneAsset> sceneReference;
    [SerializeField] private DisplayLoading loadedMode;

    public void LoadScene()
    {
        SceneLoadManager.Instance.LoadScene(sceneReference, loadedMode);
    }
}
