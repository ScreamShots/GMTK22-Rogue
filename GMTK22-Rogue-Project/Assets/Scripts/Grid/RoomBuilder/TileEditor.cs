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

    [OnValueChanged("UpdateData")]
    public TileType type = TileType.Ground;
    [OnValueChanged("UpdateData")]
    public bool canPlaceDice = true;
    [OnValueChanged("UpdateData")]
    public bool isPlayerSpawn;

    public void UpdateData()
    {
        tileRenderer.color = editor.GetTileColor(type);
        outlineRenderer.color = editor.GetSpawnColor(isPlayerSpawn);

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
        }

        tileRenderer.color = editor.GetTileColor(type);
        outlineRenderer.color = editor.GetSpawnColor(isPlayerSpawn);
    }

}
