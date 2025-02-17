using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatingCapsule
{
    [SerializeField]
    private CapsuleCollider2D capsuleCollider;
    [SerializeField]
    [Range(0.1f, 1f)]
    private float capsuleRatio = 1f;

    [SerializeField]
    private Vector2 offset = Vector2.zero;
    [SerializeField]
    private Vector2 originSize = Vector2.zero;

    private float height;

    public Vector2 OriginSize => originSize;
    public Vector2 Offset => offset;
    public float Height => height;

    public void Initialize(GameObject go)
    {
        if (capsuleCollider != null)
            return;

        capsuleCollider = go.GetComponent<CapsuleCollider2D>();

        originSize = capsuleCollider.bounds.size;
    }

    public void UpdateCapsuleDimensions()
    {
        var sizeY = Mathf.Clamp(originSize.y * capsuleRatio, originSize.x, originSize.y);
        var offsetY = originSize.y - sizeY;

        capsuleCollider.size = new Vector2(originSize.x, sizeY);
        capsuleCollider.offset = new Vector2(offset.x, offset.y + offsetY * 0.5f);

        height = originSize.y - capsuleCollider.bounds.size.y;
    }
}
