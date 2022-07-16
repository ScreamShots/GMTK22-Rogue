using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField, ReadOnly]
    TileSession occupiedTile;

    [Space]

    [SerializeField]
    float translationTime;

    public abstract void DeffineReachableTiles();

    public IEnumerator Move(TileSession destination)
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = new Vector3(destination.transform.position.x, destination.transform.position.y, transform.position.z);
        float timer = 0;

        while(timer < translationTime)
        {
            transform.Translate(Vector3.Lerp(startPoint, endPoint, timer / translationTime));
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }

        transform.position = endPoint;
        occupiedTile.SetStandinEntity(null);
        destination.SetStandinEntity(this);
        occupiedTile = destination;
    }
}
