using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameRunner : InteractionGameObject
{
    [SerializeField] private MiniGameInfoSO miniGameSoData;
    [SerializeField] private SceneAsset scene;
    [SerializeField] private Sprite interactedSprite;
    [SerializeField] private GameObject miniGameUI;

    private SpriteRenderer _renderer;
    private Sprite _spriteCache;
    private GameObject _ui;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _spriteCache = _renderer.sprite;

        GameUISet();
    }

    protected override void OnInteraction(PlayerEntity entity)
    {
        SceneManager.LoadScene(scene.name);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        _renderer.sprite = interactedSprite ?? null;
        _ui.SetActive(true);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        _renderer.sprite = _spriteCache ?? null;
        _ui.SetActive(false);
    }

    private void GameUISet()
    {
        var go = GameObject.Instantiate(miniGameUI);
        go.transform.SetParent(GameObject.Find("Canvas").transform);
        go.transform.localPosition = Vector3.zero;
        go.SetActive(false);

        _ui = go;

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
