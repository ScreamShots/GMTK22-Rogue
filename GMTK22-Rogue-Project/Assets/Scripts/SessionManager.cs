using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum SessionStates { PreDiceRoll, RollingDice ,Placement, GridPlay, GameOver, Rewards}

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
    [SerializeField]
    GlobalHUD HUD;
    [SerializeField]
    DiceManager diceManager;

    [Space]

    [SerializeField]
    List<RoomData> rooms;
    [SerializeField, ReadOnly]
    RoomData currentRoom;
    [SerializeField]
    RoomData tempDebugRoomData;

    int roomProgressionID;

    [HorizontalLine]

    [ReadOnly]
    public SessionStates currentGameState;

    [HorizontalLine]
    [SerializeField]
    int startingDiceCount;

    public event Action<SessionStates> StateEnter;
    public event Action<SessionStates> StateExit;

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
        //entityHandler.Init();
        diceManager.Initialize(startingDiceCount);
        SetSate(SessionStates.PreDiceRoll);
        //terrainHandler.ConstructNewRoom(tempDebugRoomData);
        //entityHandler.SpawnPlayer(terrainHandler.spawnTile, terrainHandler.ScaleFactor);
        //entityHandler.StartPlayerTurn();
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

    //Load Map (For now with Entities)
    //Then enable Roll dice button

    //

    public void GameLaunch()
    {
        diceManager.Initialize(startingDiceCount);
        scoreHandler.Init();
    }

    public void OnStateEnter(SessionStates newState)
    {
        StateEnter?.Invoke(newState);

        switch (newState)
        {
            case SessionStates.PreDiceRoll:
                EnterPreDiceRoll();
                break;
            case SessionStates.RollingDice:
                EnterRollingDice();
                break;
            case SessionStates.Placement:
                EnterPlacement();
                break;
            case SessionStates.GridPlay:
                break;
            case SessionStates.GameOver:
                break;
            case SessionStates.Rewards:
                break;
        }
    }

    public void OnStateExit(SessionStates lastState)
    {
        StateEnter?.Invoke(lastState);

        switch (lastState)
        {
            case SessionStates.PreDiceRoll:
                ExitPreDiceRoll();
                break;
            case SessionStates.Placement:
                break;
            case SessionStates.GridPlay:
                break;
            case SessionStates.GameOver:
                break;
            case SessionStates.Rewards:
                break;
        }
    }

    public void SetSate(SessionStates targetState)
    {
        OnStateExit(currentGameState);
        currentGameState = targetState;
        OnStateEnter(targetState);
    }

    public void EnterPreDiceRoll()
    {
        interractionHandler.UpdatePossibleInterraction(SessionStates.PreDiceRoll);
        entityHandler.Init();
        terrainHandler.ConstructNewRoom(tempDebugRoomData);
        entityHandler.SpawnPlayer(terrainHandler.spawnTile, terrainHandler.ScaleFactor);
        HUD.ToggleRoolDiceButton(true, SetSate, SessionStates.RollingDice);
    }

    public void ExitPreDiceRoll()
    {
        HUD.ToggleRoolDiceButton(false);
    }

    public void EnterRollingDice()
    {
        interractionHandler.UpdatePossibleInterraction(SessionStates.PreDiceRoll);
        diceManager.RollTheDice();
    }

    public void EnterPlacement()
    {
        interractionHandler.UpdatePossibleInterraction(SessionStates.Placement);
        HUD.ToggleDiceSlot(true);
    }
}


