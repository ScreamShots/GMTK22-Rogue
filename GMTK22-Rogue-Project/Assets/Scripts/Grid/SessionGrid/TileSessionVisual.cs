using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSessionVisual : MonoBehaviour
{
    [SerializeField]
    TileSession linkedTile;

    [SerializeField]
    SpriteRenderer tileRenderer;
    [SerializeField]
    SpriteRenderer outlineRenderer;
    [SerializeField]
    SpriteRenderer selectorRenderer;

    private void Awake()
    {
        linkedTile.HoverEnter += OnHoverEnter;
        linkedTile.HoverExit += OnHoverExit;
    }

    private void Start()
    {
        selectorRenderer.gameObject.SetActive(false);
    }

    public void OnHoverEnter()
    {
        selectorRenderer.gameObject.SetActive(true);
    }

    public void OnHoverExit()
    {
        selectorRenderer.gameObject.SetActive(false);
    }
}
