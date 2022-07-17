using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

[CreateAssetMenu(fileName = "newRoomData", menuName = "RoomBuilder/RoomData")]
public class RoomData : ScriptableObject
{
    public string roomName;
    [ReadOnly]
    public int columnsCount;
    [ReadOnly]
    public int rowsCount;

    //public TileData[,] tiles;
    public  List<TileData> tilesDataSerialization;

    public TileData GetDataFromCoords(int x, int y)
    {
        var data = tilesDataSerialization.Where(td => td.Xcoord == x && td.Ycoord == y);

        if (data.Count() < 1)
        {
            Debug.LogError("No corresponding tileData at these coordinates");
            return null;
        }
        else
            return data.First();
    }
}

public enum TileType { Ground, Wall, Door, Trap, NoDice }

[System.Serializable]
public class TileData
{
    public int Xcoord;
    public int Ycoord;
    public TileType type;
    public bool canPlaceDice;
    public bool isPlayerSpawn;
    public EntityList entityOnTile;

    public TileData((int, int) _coords)
    {
        Xcoord = _coords.Item1;
        Ycoord = _coords.Item2;
        type = TileType.Ground;
        canPlaceDice = true;
        isPlayerSpawn = false;
        entityOnTile = EntityList.None;
    }

    public void SetData((int,int) _coords, TileType _type, bool _canPlace, bool _spawn, EntityList entity)
    {
        Xcoord = _coords.Item1;
        Ycoord = _coords.Item2;
        type = _type;
        canPlaceDice = _canPlace;
        isPlayerSpawn = _spawn;
        entityOnTile = entity;
    }

    public bool Walkable => type == TileType.Ground || type == TileType.Trap || type == TileType.NoDice;
}

[System.Serializable]
public class GridParams
{
    public int rowCount;
    public int columnsCount;
    [Tooltip("Point on which is centered the bottom left tile of the grid")]
    public Transform origin;
    public float tileSize;
    public float tileSpacing;

    public GridParams(int _rowCount, int _columnsCount, Transform _origin = null, float _tileSize = 1f, float _tileSpacing = 0.2f)
    {
        rowCount = _rowCount;
        columnsCount = _columnsCount;
        origin = _origin;
        tileSize = _tileSize;
        tileSpacing = _tileSpacing;
    }
}



