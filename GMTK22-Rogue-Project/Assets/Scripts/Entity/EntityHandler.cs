using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHandler : MonoBehaviour
{
    [SerializeField, ReadOnly]
    Player currentPlayer;

    [HorizontalLine]

    [SerializeField]
    Player playerPrefab;

    public void SpawnPlayer(TileSession tileToSpawn, float scaleFactor)
    {
        currentPlayer = Instantiate(playerPrefab, transform);
        currentPlayer.Init(tileToSpawn);
        currentPlayer.transform.localScale = Vector3.one * scaleFactor;
        currentPlayer.transform.position = new Vector3(tileToSpawn.transform.position.x, tileToSpawn.transform.position.y, currentPlayer.transform.position.z);
    }

    public void StartPlayerTurn()
    {
        currentPlayer.ActionEnded -= StartPlayerTurn;
        currentPlayer.DeffinePossibleAction();
        currentPlayer.ActionStarted += OnPlayeractionStart;
    }

    public void OnPlayeractionStart()
    {
        currentPlayer.ActionStarted -= OnPlayeractionStart;
        currentPlayer.ClearPossibleAction();
        currentPlayer.ActionEnded += StartPlayerTurn;
    }

}
