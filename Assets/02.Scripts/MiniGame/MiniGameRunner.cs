using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameRunner : InteractionGameObject
{
    [SerializeField] private SceneAsset scene;
    [SerializeField] private Sprite interactedSprite;

    private SpriteRenderer _renderer;
    private Sprite _spriteCache;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _spriteCache = _renderer.sprite;
    }

    protected override void OnInteraction(PlayerEntity entity)
    {
        SceneManager.LoadScene(scene.name);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        _renderer.sprite = interactedSprite ?? null;
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        _renderer.sprite = _spriteCache ?? null;
    }
}
