using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField, ReadOnly]
    protected TileSession standingTile;

    [Space]

    public bool canTakeDamage;
    [SerializeField]
    float translationTime;

    public event Action ActionStarted;
    public event Action ActionEnded;

    public virtual void Init()
    {

    }

    public void LaunchMove(TileSession destination) => StartCoroutine(Move(destination));

    IEnumerator Move(TileSession destination)
    {
        ActionStarted?.Invoke();

        Vector3 startPoint = transform.position;
        Vector3 endPoint = new Vector3(destination.transform.position.x, destination.transform.position.y, transform.position.z);
        float timer = 0;

        while(timer < translationTime)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, timer / translationTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        transform.position = endPoint;
        standingTile.SetStandinEntity(null);
        destination.SetStandinEntity(this);
        standingTile = destination;

        ActionEnded?.Invoke();
    }
}
