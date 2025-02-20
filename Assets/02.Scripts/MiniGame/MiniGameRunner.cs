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
    private UIMiniGameBoard gameUI;

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
        if (gameUI == null)
            return;

        base.OnTriggerEnter2D(other);
        sprRenderer.sprite = interactedSprite ?? null;

        gameUI.gameObject.SetActive(true);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (gameUI == null)
            return;

        base.OnTriggerExit2D(other);
        sprRenderer.sprite = spriteCache ?? null;
        gameUI.gameObject.SetActive(false);
    }

    private void GameUISet()
    {
        var go = GameObject.Instantiate(miniGameUI);
        go.transform.SetParent(UIManager.Instance.GetCanvas().transform);
        go.transform.localPosition = Vector3.zero;
        go.SetActive(false);

        gameUI = go.GetComponent<UIMiniGameBoard>();

        if (miniGameSoData == null)
            return;

        gameUI.TitleTmp.text = miniGameSoData.gameName;
        gameUI.DescTmp.text = miniGameSoData.gameDescription;

        string rankingStr = string.Empty;

        //수정필요
        foreach (var iter in SaveLoader.Instance.data.flappyBirdData.rankingScoreList)
        {
            rankingStr += iter.ToString() + "\n\n";
        }

        gameUI.RankingTmp.text = rankingStr;
    }

    private void LoadRanking()
    {
        if (miniGameSoData == null)
            return;


    }
}



