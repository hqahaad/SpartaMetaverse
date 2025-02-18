using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameRunner : InteractionGameObject
{
    [SerializeField] private SceneAsset scene;

    protected override void OnInteraction(PlayerEntity entity)
    {
        SceneManager.LoadScene(scene.name);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
