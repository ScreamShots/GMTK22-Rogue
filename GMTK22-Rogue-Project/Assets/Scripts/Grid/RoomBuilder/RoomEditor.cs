using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class RoomEditor : MonoBehaviour
{
    [SerializeField, Expandable]
    RoomData currentData;
    public bool autoSave;

    [HorizontalLine]
    [SerializeField, Expandable]
    DebugTileColors textures;
    [SerializeField]
    GridParams generationParameters;
    [SerializeField]
    TileEditor tileEditorTemplate;

    [SerializeField]
    List<TileEditor> allTiles;


    [Button("Create a Blank Grid")]
    public void GenerateNewGrid()
    {
        if (currentData == null)
        {
            Debug.LogWarning("Assign a data container before creating a new Grid");
            return;
        }

        var sizeT = tileEditorTemplate.GetTileSize();
        float tileXSize = sizeT.Item1;
        float tileYSize = sizeT.Item2;
        allTiles = new List<TileEditor>();

        Vector3 posToSpawn = Vector3.zero;
        TileEditor _t;

        currentData.columnsCount = generationParameters.columnsCount;
        currentData.rowsCount = generationParameters.rowCount;
        currentData.roomName = "newRoom";

        //currentData.tiles = new TileData[generationParameters.columnsCount, generationParameters.rowCount];
        currentData.tilesDataSerialization = new List<TileData>();

        for (int x = 0; x < generationParameters.columnsCount; x++)
        {
            for (int y = 0; y < generationParameters.rowCount; y++)
            {
                _t = Instantiate(tileEditorTemplate, posToSpawn, Quaternion.identity, transform);
                _t.SetCoords(x, y);
                _t.SetupdData(this);
                allTiles.Add(_t);

                currentData.tilesDataSerialization.Add(new TileData((x, y)));
                //currentData.tiles[x, y] = new TileData();
                UpdateTileData(_t);

                posToSpawn.y += (tileYSize + generationParameters.tileSpacing);
            }

            posToSpawn.x += (tileXSize + generationParameters.tileSpacing);
            posToSpawn.y = 0;
        }
    }

    [Button("Load Grid from assigned Data")]
    public void LoadFromData()
    {
        if (currentData == null)
        {
            Debug.LogWarning("Assign a data container in order to load from it");
            return;
        }

        try
        {
            ClearBuilder();

            var sizeT = tileEditorTemplate.GetTileSize();
            float tileXSize = sizeT.Item1;
            float tileYSize = sizeT.Item2;
            allTiles = new List<TileEditor>();

            Vector3 posToSpawn = Vector3.zero;
            TileEditor _t;

            if (currentData.columnsCount <= 0 && currentData.rowsCount <= 0)
            {
                Debug.LogWarning("the data container is Empty");
                return;
            }

            for (int x = 0; x < currentData.columnsCount; x++)
            {
                for (int y = 0; y < currentData.rowsCount; y++)
                {
                    _t = Instantiate(tileEditorTemplate, posToSpawn, Quaternion.identity, transform);
                    _t.SetCoords(x, y);
                    //_t.SetupdData(this, currentData.tiles[x, y]);
                    _t.SetupdData(this, currentData.GetDataFromCoords(x, y));
                    allTiles.Add(_t);

                    posToSpawn.y += (tileYSize + generationParameters.tileSpacing);
                }

                posToSpawn.x += (tileXSize + generationParameters.tileSpacing);
                posToSpawn.y = 0;
            }
        }
        catch
        {
            Debug.LogError("Unable to load from the data container, it could be corrupted");
        }

    }

    [Button("Save grid into assigned Data")]
    public void SaveGrid()
    {
        if (currentData == null)
        {
            Debug.LogWarning("Assign a data container in order to save the grid into it");
            return;
        }
        foreach (TileEditor t in allTiles)
            UpdateTileData(t);
    }

    [Button("Clear all")]
    public void ClearBuilder()
    {
        foreach (TileEditor t in allTiles.ToList())
            DestroyImmediate(t.gameObject);
        allTiles = new List<TileEditor>();
    }

    public void UpdateTileData(TileEditor t)
    {
        if (currentData == null)
        {
            Debug.LogWarning("Can't save tile Data because their is not container");
            return;
        }
        try
        {
            //currentData.tiles[t.coords[0], t.coords[1]]
            currentData.GetDataFromCoords(t.coords[0], t.coords[1])
            .SetData(
                (t.coords[0], t.coords[1]),
                t.type,
                t.canPlaceDice,
                t.isPlayerSpawn);
        }
        catch { Debug.LogError("Unable to save tile data. Data container may be corrupted"); }
    }

    public Color GetTileColor(TileType type)
    {
        return textures.colorPalette.Where(i => i.type == type).First().color;
    }

    internal Color GetSpawnColor(bool isSpawn)
    {
        Color outlineColor = isSpawn ? textures.spawnOutlineColor : textures.defaultOutlineColor;
        return outlineColor;
    }
}
