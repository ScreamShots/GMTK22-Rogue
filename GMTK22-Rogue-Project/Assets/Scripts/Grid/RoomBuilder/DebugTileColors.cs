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

    [System.Serializable]
    public struct EntitySpritePair
    {
        public EntityList entity;
        public Sprite sprite;
    }

    public Color GetColorFromType(TileType type)
    {
        return colorPalette.Where(i => i.type == type).First().color;
    }

    public Sprite GetEntitySprite(EntityList type)
    {
        return entityAtlas.Where(i => i.entity == type).First().sprite;
    }

    public List<TileColorPair> colorPalette;
    public List<EntitySpritePair> entityAtlas;

    public Color spawnOutlineColor;
    public Color defaultOutlineColor;
    public Color walkableOutlineColor;
    public Color attackableOutlineColor;

    public Color openDoorColor;
    public Color closeDoorColor;
}
