using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "newTileDebugTexture", menuName = "TileTexture/DebugTexturePack")]
public class DebugTileColors : ScriptableObject
{
    [System.Serializable]
    public struct TileColorPair
    {
        public TileType type;
        public Color color;
    }

    public Color GetColorFromType(TileType type)
    {
        return colorPalette.Where(i => i.type == type).First().color;
    } 

    public List<TileColorPair> colorPalette;

    public Color spawnOutlineColor;
    public Color defaultOutlineColor;
    public Color walkableOutlineColor;
    public Color attackableOutlineColor;
}
