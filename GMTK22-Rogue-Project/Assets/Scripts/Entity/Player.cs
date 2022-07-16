using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : EntityBase
{
    List<TileSession> attackableTiles;
    List<TileSession> walkableTiles;

    public void Init(TileSession firstStandingTile)
    {
        base.Init();

        attackableTiles = new List<TileSession>();
        walkableTiles = new List<TileSession>();

        standingTile = firstStandingTile;
        firstStandingTile.SetStandinEntity(this);
    }

    public void DeffinePossibleAction()
    {
        void TestNerby(int xOffset, int yOffset)
        {
            (int, int) coordsToPick = (standingTile.coords[0] + xOffset, standingTile.coords[1] + yOffset);
            bool possibleXCoords = coordsToPick.Item1 >= 0 && coordsToPick.Item1 < TerrainHandler.currentGrid.GetLength(0);
            bool possibleYCoords = coordsToPick.Item2 >= 0 && coordsToPick.Item2 < TerrainHandler.currentGrid.GetLength(1);

            if (possibleXCoords && possibleYCoords)
            {
                var pickedTile = TerrainHandler.currentGrid[coordsToPick.Item1, coordsToPick.Item2];

                if (pickedTile.Walkable)
                {
                    walkableTiles.Add(pickedTile);
                    pickedTile.SetPossibleActionOnCells(PossibleAction.Walkable);
                    pickedTile.RightClick += LaunchMove;
                }
                else if (pickedTile.Attackable)
                {
                    attackableTiles.Add(pickedTile);
                    pickedTile.SetPossibleActionOnCells(PossibleAction.Attackable);
                    //pickedTile.RightClick += LaunchAttack;
                }
            }
        }

        TestNerby(-1, 0);
        TestNerby(1, 0);
        TestNerby(0, -1);
        TestNerby(0, 1);
    }

    public void ClearPossibleAction()
    {
        foreach (var at in attackableTiles)
        {
            at.SetPossibleActionOnCells(PossibleAction.None);
            //at.RightClick -= LaunchAttack
        }

        foreach (var wt in walkableTiles)
        {
            wt.SetPossibleActionOnCells(PossibleAction.None);
            wt.RightClick -= LaunchMove;
        }
    }
}
