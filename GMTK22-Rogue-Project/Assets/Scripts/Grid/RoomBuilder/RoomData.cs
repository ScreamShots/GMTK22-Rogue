using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "newRoomData", menuName = "RoomBuilder/RoomData")]
public class RoomData : ScriptableObject
{
    public string roomName;
    [ReadOnly]
    public int columnsCount;
    [ReadOnly]
    public int rowsCount;

    [SerializeField]
    public TileData[,] tiles;
}

public enum TileType { Wall, Door, Ground }

[System.Serializable]
public class TileData
{
    public (int, int) coords { get; protected set; }
    public TileType type { get; protected set; }
    public bool canPlaceDice { get; protected set; }
    public bool isPlayerSpawn { get; protected set; }

    public void SetData((int,int) _coords, TileType _type, bool _canPlace, bool _spawn)
    {
        coords = _coords;
        type = _type;
        canPlaceDice = _canPlace;
        isPlayerSpawn = _spawn;
    }
}



