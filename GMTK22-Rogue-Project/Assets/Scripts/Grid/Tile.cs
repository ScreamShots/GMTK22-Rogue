using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer baseTileSprite;
    [SerializeField]
    SpriteRenderer outlineTileSprite;
    [SerializeField]
    SpriteRenderer selectionTileSprite;
    [SerializeField]
    Collider2D colliderBox;

    [SerializeField, ReadOnly]
    int[] coords = new int[2];


    public (float, float) GetTileSize() => (baseTileSprite.bounds.size.x, baseTileSprite.bounds.size.y);
    public void SetCoords(int x, int y) 
    {
        coords = new int[2];
        coords[0] = x;
        coords[1] = y;
    }

}