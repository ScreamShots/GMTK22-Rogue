using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileSession : Tile
{
    [SerializeField, ReadOnly]
    TileData linkedData;
    [SerializeField, ReadOnly]
    EntityBase currentStandingEntity; 

    [SerializeField]
    TileSessionVisual visuals;

    //Interraction actions;
    public event Action HoverEnter;
    public event Action HoverExit;
    public event Action RightClick;
    public event Action LeftClick;

    public void InitTile()
    {

    }

    public void ClearTile()
    {

    }

    public void CallHoverEnter() => HoverEnter?.Invoke();
    public void CallHoverExit() => HoverExit?.Invoke();

    public void SetStandinEntity(EntityBase entity)
    {
        if(currentStandingEntity != null && entity != null)
        {
            Debug.LogError("Already a standing unity on this tile");
            return;
        }

        currentStandingEntity = entity;

    }
}
