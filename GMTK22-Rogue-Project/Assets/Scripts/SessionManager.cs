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

    [Space]

    [SerializeField]
    RoomData tempDbugRoomData;

    private void Start()
    {
        terrainHandler.ConstructNewRoom(tempDbugRoomData);
    }
}


