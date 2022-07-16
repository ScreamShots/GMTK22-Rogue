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
    [SerializeField]
    DebugTileColors colorPalette;

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

    public void SetTileTexture(TileType type)
    {
        tileRenderer.color = colorPalette.GetColorFromType(type);
    }

    
    public void SetPossibleActionFeedback(PossibleAction type)
    {
        switch (type)
        {
            case PossibleAction.Walkable:
                outlineRenderer.color = colorPalette.walkableOutlineColor;
                break;
            case PossibleAction.Attackable:
                outlineRenderer.color = colorPalette.attackableOutlineColor;
                break;
            case PossibleAction.None:
                outlineRenderer.color = colorPalette.defaultOutlineColor;
                break;
        }
    }
}
