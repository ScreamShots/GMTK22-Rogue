using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : EntityBase
{
    enum MovementDirection{Est, West, North, South}
    [SerializeField]
    List<MovementDirection> movementPatern;
    [SerializeField, ReadOnly]
    int paternStep;

    [HorizontalLine]

    [SerializeField]
    MonsterVisual visuals;

    public void CalculateNextAction()
    {
        (int, int) lookAtTileCoords = (0,0);
        TileSession lookAtTile;

        switch (movementPatern[paternStep])
        {
            case MovementDirection.Est:
                lookAtTileCoords = (standingTile.coords[0] + 1, standingTile.coords[1]);
                break;
            case MovementDirection.West:
                lookAtTileCoords = (standingTile.coords[0] -1, standingTile.coords[1]);
                break;
            case MovementDirection.North:
                lookAtTileCoords = (standingTile.coords[0], standingTile.coords[1] + 1);
                break;
            case MovementDirection.South:
                lookAtTileCoords = (standingTile.coords[0], standingTile.coords[1] -1);
                break;
        }

        bool possibleXCoords = lookAtTileCoords.Item1 >= 0 && lookAtTileCoords.Item1 < TerrainHandler.currentGrid.GetLength(0);
        bool possibleYCoords = lookAtTileCoords.Item2 >= 0 && lookAtTileCoords.Item2 < TerrainHandler.currentGrid.GetLength(1);


        if (possibleXCoords && possibleYCoords)
        {
            lookAtTile = TerrainHandler.currentGrid[lookAtTileCoords.Item1, lookAtTileCoords.Item2];

            if (lookAtTile.Walkable)
            {
                LaunchMove(lookAtTile);
            }
            else
            {
                LaunchAttack(lookAtTile);
            }
        }
        else
        {
            CallActionStart();
            CallActionEnd();
        }

        paternStep++;
        if (paternStep >= movementPatern.Count)
            paternStep = 0;
    }

    protected override void OnOpponentDeathBehavior(Action BehaviorEnd, EntityBase target)
    {
        void ResumeGameplay(object obj = null, EventArgs args = null)
        {
            target.DeathEnds -= ResumeGameplay;
            BehaviorEnd?.Invoke();
        }

        target.DeathEnds += ResumeGameplay;
    }

    protected override void OnDeath()
    {
        CallDeathStart();
        visuals.OnDeathAnimation(DropItems);
    }

    void DropItems()
    {
        SessionManager.Instance.AddScore(scoreOnKill);
        Obliterate();
    }

    void Obliterate()
    {
        standingTile.SetStandinEntity(null);
        SessionManager.Instance.KillEntity(this);
        CallDeathEnd();
    }
}
