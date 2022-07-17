using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using System.Linq;

public class TileEditor : Tile
{
    RoomEditor editor;
    [SerializeField]
    protected SpriteRenderer outlineRenderer;
    [SerializeField]
    protected SpriteRenderer entityRenderer;

    [OnValueChanged("UpdateData")]
    public TileType type = TileType.Ground;
    [OnValueChanged("UpdateData")]
    public bool canPlaceDice = true;
    [OnValueChanged("UpdateData")]
    public bool isPlayerSpawn;
    [OnValueChanged("UpdateData")]
    public EntityList entityOnTile;

    public void UpdateData()
    {
        tileRenderer.color = editor.GetTileColor(type);
        outlineRenderer.color = editor.GetSpawnColor(isPlayerSpawn);
        entityRenderer.sprite = editor.GetEntitySprite(entityOnTile);

        if (editor.autoSave)
            editor.UpdateTileData(this);
    }

    public void SetupdData(RoomEditor _editor, TileData data = null)
    {
        editor = _editor;

        if(data != null)
        {
            type = data.type;
            canPlaceDice = data.canPlaceDice;
            isPlayerSpawn = data.isPlayerSpawn;
            entityOnTile = data.entityOnTile;
        }

        tileRenderer.color = editor.GetTileColor(type);
        outlineRenderer.color = editor.GetSpawnColor(isPlayerSpawn);
        entityRenderer.sprite = editor.GetEntitySprite(entityOnTile);
    }

}
