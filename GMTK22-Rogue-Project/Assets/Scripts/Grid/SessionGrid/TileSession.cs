using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PossibleAction { Walkable, Attackable, None }

public class TileSession : Tile
{
    [SerializeField, ReadOnly]
    TileData linkedData;
    [SerializeField, ReadOnly]
    EntityBase currentStandingEntity; 
    [SerializeField]
    TileSessionVisual visuals;
    [SerializeField, ReadOnly]
    PossibleAction possibleActionOnCell;

    [HorizontalLine]

    [SerializeField]
    float trapDmg;


    //Interraction actions;
    public event Action HoverEnter;
    public event Action HoverExit;
    public event Action<TileSession> RightClick;
    public event Action<TileSession> LeftClick;

    public bool Walkable => (linkedData.Walkable && currentStandingEntity == null) || (linkedData.type == TileType.Door && TerrainHandler.doorState);
    public bool Attackable => currentStandingEntity != null && currentStandingEntity.canTakeDamage;
    public bool IsSpawn => linkedData.isPlayerSpawn;

    public void InitTile(TileData data)
    {
        linkedData = data;
        possibleActionOnCell = PossibleAction.None;
        visuals.SetTileTexture(data.type);
    }

    public void ClearTile()
    {

    }

    public void SpawnEntity()
    {
        if (linkedData.entityOnTile != EntityList.None)
            SessionManager.Instance.SpawnEntity(this, linkedData.entityOnTile, transform.localScale.x);
    }

    public void CallHoverEnter() => HoverEnter?.Invoke();
    public void CallHoverExit() => HoverExit?.Invoke();
    public void CallRightClick() => RightClick?.Invoke(this);

    public void SetStandinEntity(EntityBase entity)
    {
        if(currentStandingEntity != null && entity != null)
        {
            Debug.LogError("Already a standing unity on this tile");
            return;
        }

        currentStandingEntity = entity;

        if(linkedData.type == TileType.Trap)
        {
            currentStandingEntity?.TakeDamages(trapDmg);
        }
        if(linkedData.type == TileType.Door)
        {
            SessionManager.Instance.GoToNextRoom();
        }
    }

    public EntityBase GetStandingEntity() => currentStandingEntity;
    public TileType GetTileType() => linkedData.type;

    public void SetPossibleActionOnCells(PossibleAction type)
    {
        possibleActionOnCell = type;
        visuals.SetPossibleActionFeedback(type);
    }

    public void UpdateDoor(bool state)
    {
        visuals.SetTileTexture(linkedData.type, state);
    }
}
