using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTileDebugTexture", menuName = "TileTexture/DebugTexturePack")]
public class DebugTileColors : ScriptableObject
{
    [System.Serializable]
    public struct TileColorPair
    {
        public TileType type;
        public Color color;
    }

    public List<TileColorPair> colorPalette;
    public Color spawnOutlineColor;
    public Color defaultOutlineColor;
}
