using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : EntityBase
{
    [SerializeField]
    ChestVisual visuals;

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

    protected override void OnOpponentDeathBehavior(Action BehaviorEnd, EntityBase target)
    {
    }
}
