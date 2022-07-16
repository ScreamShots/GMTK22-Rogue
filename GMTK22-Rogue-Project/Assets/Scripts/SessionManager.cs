using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    TerrainHandler terrainHandler;
    [SerializeField]
    InterractionHandler interractionHandler;
    [SerializeField]
    EntityHandler entityHandler;

    [Space]

    [SerializeField]
    RoomData tempDbugRoomData;

    private void Start()
    {
        terrainHandler.ConstructNewRoom(tempDbugRoomData);
        entityHandler.SpawnPlayer(terrainHandler.spawnTile, terrainHandler.ScaleFactor);
        entityHandler.StartPlayerTurn();
    }
}


