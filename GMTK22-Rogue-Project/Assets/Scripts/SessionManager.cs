using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance; 

    [SerializeField]
    TerrainHandler terrainHandler;
    [SerializeField]
    InterractionHandler interractionHandler;
    [SerializeField]
    EntityHandler entityHandler;
    [SerializeField]
    ScoreHandler scoreHandler;

    [Space]

    [SerializeField]
    RoomData tempDebugRoomData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        scoreHandler.Init();
        entityHandler.Init();
        terrainHandler.ConstructNewRoom(tempDebugRoomData);
        entityHandler.SpawnPlayer(terrainHandler.spawnTile, terrainHandler.ScaleFactor);
        entityHandler.StartPlayerTurn();
    }

    public void SpawnEntity(TileSession tileToSpawn, EntityList type, float scaleFactor) => entityHandler.SpawnEntity(tileToSpawn, type, scaleFactor);
    public void KillEntity(EntityBase entity) => entityHandler.RemoveEntity(entity);
    public void AddScore(float scoreToAdd) => scoreHandler.AddScore(scoreToAdd);
    public void CallDoorUpdate(bool state) => terrainHandler.ControlDoors(state);

    public void GoToNextRoom()
    {
        //TempDebug
        entityHandler.ResetEntities(false);
        interractionHandler.ResetSelectionModels();
        entityHandler.Init();
        terrainHandler.ConstructNewRoom(tempDebugRoomData);
        entityHandler.SpawnPlayer(terrainHandler.spawnTile, terrainHandler.ScaleFactor);
        entityHandler.StartPlayerTurn();
    }

    public void GameOver()
    {
        //Temp debug
        scoreHandler.ResetScore();
        interractionHandler.ResetSelectionModels();
        entityHandler.ResetEntities(false);
        entityHandler.Init();
        terrainHandler.ConstructNewRoom(tempDebugRoomData);
        entityHandler.SpawnPlayer(terrainHandler.spawnTile, terrainHandler.ScaleFactor);
        entityHandler.StartPlayerTurn();
    }
}


