using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField]
    DebugFillableBar debugHpBar;
    [SerializeField]
    bool showdebugHpBar;

    [HorizontalLine]
    [SerializeField, ReadOnly]
    protected TileSession standingTile;

    [Space]

    public bool canTakeDamage;
    [SerializeField]
    public float baseHp;
    [SerializeField, ReadOnly]
    float currentHp;
    [SerializeField]
    public float baseAttack;
    [SerializeField, ReadOnly]
    public float currentAttack;
    [SerializeField]
    float translationTime;
    [SerializeField]
    float attackTime;
    [SerializeField]
    protected float scoreOnKill;

    public event EventHandler ActionStarted;
    public event EventHandler ActionEnded;
    public event EventHandler DeathStart;
    public event EventHandler DeathEnds;
    /// <summary>
    /// f2: newValue;
    /// f3: maxHp;
    /// </summary>
    public event Action<float, float> TakingDamage;
    public event Action<float, float> Healing;

    public virtual void Init(TileSession firstStandingTile)
    {
        standingTile = firstStandingTile;
        firstStandingTile.SetStandinEntity(this);

        currentAttack = baseAttack;
        currentHp = baseHp;

        if (debugHpBar != null)
        {
            if (showdebugHpBar)
            {
                TakingDamage += debugHpBar.UpdateFillable;
                Healing += debugHpBar.UpdateFillable;
            }
            else
                debugHpBar.gameObject.SetActive(false);
        }

    }

    private void OnDestroy()
    {
        ClearDelegates();
    }

    public virtual void ClearDelegates()
    {
        ActionStarted = null;
        ActionEnded = null;
        DeathStart = null;
        DeathEnds = null;
        TakingDamage = null;
        Healing = null;
    }

    public void UnlinkFromTiles() => standingTile.SetStandinEntity(null);

    public void LaunchMove(TileSession destination) => StartCoroutine(Move(destination));
    public void LaunchAttack(TileSession target) => StartCoroutine(Attack(target));
    protected void CallActionStart() => ActionStarted?.Invoke(this, EventArgs.Empty);
    protected void CallActionEnd() => ActionEnded?.Invoke(this, EventArgs.Empty);
    protected void CallDeathStart() => DeathStart?.Invoke(this, EventArgs.Empty);
    protected void CallDeathEnd() => DeathEnds?.Invoke(this, EventArgs.Empty);



    IEnumerator Move(TileSession destination)
    {
        ActionStarted?.Invoke(this, EventArgs.Empty);

        Vector3 startPoint = transform.position;
        Vector3 endPoint = new Vector3(destination.transform.position.x, destination.transform.position.y, transform.position.z);
        float timer = 0;

        while (timer < translationTime)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, timer / translationTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        transform.position = endPoint;
        standingTile.SetStandinEntity(null);
        destination.SetStandinEntity(this);
        standingTile = destination;

        ActionEnded?.Invoke(this, EventArgs.Empty);
    }

    IEnumerator Attack(TileSession target)
    {
        bool waitDeath;
        void EndWaitDeath() => waitDeath = false;

        ActionStarted?.Invoke(this, EventArgs.Empty);

        Vector3 startPoint = transform.position;
        Vector3 endPoint = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        float timer = 0;

        while (timer < attackTime / 2)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, timer / attackTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        while (timer > 0)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, timer / attackTime);
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }

        transform.position = startPoint;

        if (target.GetStandingEntity() != null)
        {
            if (target.GetStandingEntity().TakeDamages(currentAttack))
            {
                waitDeath = true;
                OnOpponentDeathBehavior(EndWaitDeath, target.GetStandingEntity());

                while (waitDeath)
                    yield return new WaitForEndOfFrame();
            }
        }

        ActionEnded?.Invoke(this, EventArgs.Empty);
    }

    protected abstract void OnOpponentDeathBehavior(Action BehaviorEnd, EntityBase target);

    public bool TakeDamages(float dmgValue)
    {
        currentHp -= dmgValue;
        bool dead = false;

        if (currentHp <= 0)
        {
            dead = true;
            OnDeath();
        }

        TakingDamage?.Invoke(currentHp, baseHp);
        return dead;
    }

    public void Heal(float healValue)
    {
        currentHp += healValue;

        if (currentHp > baseHp)
            currentHp = baseHp;

        Healing?.Invoke(currentHp, baseHp);
    }

    protected abstract void OnDeath();

}
