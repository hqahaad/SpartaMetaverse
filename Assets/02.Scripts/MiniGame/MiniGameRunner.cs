using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

public class MiniGameRunner : InteractionGameObject
{
    [SerializeField] private MiniGameInfoSO miniGameSoData;
    [SerializeField] private AssetReferenceT<SceneAsset> scene;
    [SerializeField] private Sprite interactedSprite;
    [SerializeField] private GameObject miniGameUI;

    private SpriteRenderer sprRenderer;
    private Sprite spriteCache;
    private GameObject gameUI;

    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        spriteCache = sprRenderer.sprite;

        GameUISet();
    }

    protected override void OnInteraction(PlayerEntity entity)
    {
        SceneLoadManager.Instance.LoadScene(scene);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        sprRenderer.sprite = interactedSprite ?? null;
        gameUI.SetActive(true);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        sprRenderer.sprite = spriteCache ?? null;
        gameUI.SetActive(false);
    }

    private void GameUISet()
    {
        var go = GameObject.Instantiate(miniGameUI);
        go.transform.SetParent(UIManager.Instance.GetCanvas().transform);
        go.transform.localPosition = Vector3.zero;
        go.SetActive(false);

        gameUI = go;

        if (miniGameSoData == null)
            return;

        //임시코드 나중에 수정하자
        var tmps = go.transform.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var iter in tmps)
        {
            if (iter.gameObject.name == "TitleTMP")
            {
                iter.text = miniGameSoData.gameName;
            }
            else if (iter.gameObject.name == "DescTMP")
            {
                iter.text = miniGameSoData.gameDescription;
            }
        }
    }

    private void LoadRanking()
    {
        if (miniGameSoData == null)
            return;


    }
}
