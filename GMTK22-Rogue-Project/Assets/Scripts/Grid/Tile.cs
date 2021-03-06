using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [ReadOnly]
    public int[] coords;
    [SerializeField]
    protected SpriteRenderer tileRenderer;

    public (float, float) GetTileSize() => (tileRenderer.bounds.size.x, tileRenderer.bounds.size.y);
    public void SetCoords(int x, int y) 
    {
        coords = new int[2];
        coords[0] = x;
        coords[1] = y;
    }

}
