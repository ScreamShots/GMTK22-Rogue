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

    [SerializeField] private Sprite groundSprite;
    [SerializeField] private Sprite wallSprite;
    [SerializeField] private Sprite noDiceSprite;
    [SerializeField] private Sprite doorCloseSprite;
    [SerializeField] private Sprite doorOpenSprite;

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

    public void SetTileTexture(TileType type, bool doorState = false)
    {
        switch (type)
        {
            case TileType.Ground:
                tileRenderer.sprite = groundSprite;
                break;
            case TileType.Wall:
                tileRenderer.sprite = wallSprite;
                break;
            case TileType.Door:
                tileRenderer.sprite = doorState ? doorOpenSprite : doorCloseSprite;
                break;
            case TileType.Trap:
                break;
            case TileType.NoDice:
                tileRenderer.sprite = noDiceSprite;
                break;
            default:
                break;
        }

        tileRenderer.transform.localScale = new Vector2(0.275f, 0.275f);

        //if (type == TileType.Door)
        //    tileRenderer.color = doorState ? colorPalette.openDoorColor : colorPalette.closeDoorColor;
        //else
        //    tileRenderer.color = colorPalette.GetColorFromType(type);
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
                outlineRenderer.color = Color.clear;
                break;
        }
    }
}
