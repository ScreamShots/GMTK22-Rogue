using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections.ObjectModel;

public enum EntityList { None, Chest, Monster }

public class EntityHandler : MonoBehaviour
{
    [SerializeField, ReadOnly]
    Player currentPlayer;
    [SerializeField]
    ObservableCollection<Monster> allMonsters;
    [SerializeField]
    List<Chest> allChests;

    [HorizontalLine]

    [SerializeField]
    float timeBetweenTurn;

    [HorizontalLine]

    [SerializeField]
    Player playerPrefab;
    [SerializeField]
    Monster monsterPrefab;
    [SerializeField]
    Chest chestPrefab;

    public void Init()
    {
        allMonsters = new ObservableCollection<Monster>();
        allChests = new List<Chest>();
        allMonsters.CollectionChanged += OnMonsterCollectionChanged;
    }

    public void OnMonsterCollectionChanged(object obj = null, EventArgs args = null)
    {
        StopAllCoroutines();
        if (allMonsters.Count == 0 && !TerrainHandler.doorState)
            SessionManager.Instance.CallDoorUpdate(true);
        else if (allMonsters.Count > 0 && TerrainHandler.doorState)
            SessionManager.Instance.CallDoorUpdate(false);
    }

    public void ResetEntities(bool keepGrid)
    {
        if (keepGrid)
            currentPlayer.UnlinkFromTiles();
        currentPlayer.ClearDelegates();
        Destroy(currentPlayer.gameObject);

        for (int i = 0; i < allMonsters.Count; i++)
        {
            if (keepGrid)
                allMonsters[i].UnlinkFromTiles();
            allMonsters[i].ClearDelegates();
            Destroy(allMonsters[i].gameObject);
        }

        for (int i = 0; i < allChests.Count; i++)
        {
            if (keepGrid)
                allChests[i].UnlinkFromTiles();
            allChests[i].ClearDelegates();
            Destroy(allChests[i].gameObject);
        }

        allMonsters = new ObservableCollection<Monster>();
        allChests = new List<Chest>();

        allMonsters.CollectionChanged += OnMonsterCollectionChanged;
    }

    public void SpawnPlayer(TileSession tileToSpawn, float scaleFactor)
    {
        currentPlayer = Instantiate(playerPrefab, transform);
        currentPlayer.Init(tileToSpawn);
        currentPlayer.transform.localScale = Vector3.one * scaleFactor;
        currentPlayer.transform.position = new Vector3(tileToSpawn.transform.position.x, tileToSpawn.transform.position.y, currentPlayer.transform.position.z);
    }

    public void SpawnEntity(TileSession tileToSpawn, EntityList type, float scaleFactor)
    {
        switch (type)
        {
            case EntityList.Chest:
                var c = Instantiate(chestPrefab, transform);
                c.Init(tileToSpawn);
                allChests.Add(c);
                c.transform.localScale = Vector3.one * scaleFactor;
                c.transform.position = new Vector3(tileToSpawn.transform.position.x, tileToSpawn.transform.position.y, c.transform.position.z);
                break;
            case EntityList.Monster:
                var m = Instantiate(monsterPrefab, transform);
                m.Init(tileToSpawn);
                allMonsters.Add(m);
                m.transform.localScale = Vector3.one * scaleFactor;
                m.transform.position = new Vector3(tileToSpawn.transform.position.x, tileToSpawn.transform.position.y, m.transform.position.z);
                break;
        }
    }

    public void RemoveEntity(EntityBase entity)
    {
        if(entity is Monster)
        {
            allMonsters.Remove(entity as Monster);
            allMonsters = new ObservableCollection<Monster>(allMonsters.Where(m => m != null));
            allMonsters.CollectionChanged += OnMonsterCollectionChanged;
        }
        else if(entity is Chest)
        {
            allChests.Remove(entity as Chest);
            allChests = allChests.Where(m => m != null).ToList();
        }

        Destroy(entity.gameObject);
    }

    public void StartPlayerTurn(object obj = null, EventArgs args = null)
    {
        currentPlayer.ActionEnded -= StartPlayerTurn;
        currentPlayer.DeffinePossibleAction();
        currentPlayer.ActionStarted += OnPlayerActionStart;
    }

    public void OnPlayerActionStart(object obj = null, EventArgs args = null)
    {
        currentPlayer.ActionStarted -= OnPlayerActionStart;
        currentPlayer.ClearPossibleAction();
        currentPlayer.ActionEnded += StartMonsterTurn;
    }

    public void StartMonsterTurn(object obj = null, EventArgs args = null)
    {
        currentPlayer.ActionEnded -= StartMonsterTurn;

        if (allMonsters.Count == 0)
        {
            StartPlayerTurn();
            return;
        }
        else
            StartCoroutine(MonstersTurn());
    }

    public IEnumerator MonstersTurn()
    {
        bool canContinue = false;

        void SetContinue(object obj = null, EventArgs args = null)
        {
            canContinue = true;
        }

        yield return new WaitForSeconds(timeBetweenTurn);

        for (int i = 0; i < allMonsters.Count; i++)
        {
            if (allMonsters[i] == null)
                continue;

            canContinue = false;
            allMonsters[i].ActionEnded += SetContinue;
            allMonsters[i].CalculateNextAction();

            while (!canContinue)
                yield return new WaitForEndOfFrame();

            allMonsters[i].ActionEnded -= SetContinue;

            yield return new WaitForSeconds(timeBetweenTurn);
        }

        StartPlayerTurn();
    }

}
