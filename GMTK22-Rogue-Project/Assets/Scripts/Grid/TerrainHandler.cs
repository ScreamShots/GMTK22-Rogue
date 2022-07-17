using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainHandler : MonoBehaviour
{
    [SerializeField, ReadOnly]
    RoomData currentRoomData;
    public static TileSession[,] currentGrid;
    public static bool doorState;
    public TileSession spawnTile => currentGrid.Cast<TileSession>().ToList().Where(t => t.IsSpawn).First();

    [Space]
    [SerializeField]
    Transform gridCenter;
    [SerializeField]
    Transform gridParent;
    [SerializeField]
    TileSession tileSessionTemplate;
    [SerializeField]
    float tileSpacing;
    [SerializeField]
    float tileSize;
    public float ScaleFactor => tileSize;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private AudioClip doorCloseClip;

    public void ConstructNewRoom(RoomData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Assign a data container in order to load from it");
            return;
        }

        try
        {
            if (currentGrid != null)
                ClearRoom();

            currentRoomData = data;

            tileSessionTemplate.transform.localScale = Vector3.one * tileSize;
            var sizeT = tileSessionTemplate.GetTileSize();
            float tileXSize = sizeT.Item1;
            float tileYSize = sizeT.Item2;
            currentGrid = new TileSession[data.columnsCount, data.rowsCount];

            Vector3 posToSpawn = gridCenter.position;
            TileSession _t;

            if (data.columnsCount <= 0 && data.rowsCount <= 0)
            {
                Debug.LogWarning("the data container is Empty");
                return;
            }

            for (int x = 0; x < data.columnsCount; x++)
            {
                for (int y = 0; y < data.rowsCount; y++)
                {
                    _t = Instantiate(tileSessionTemplate, posToSpawn, Quaternion.Euler(90,0,0), gridParent);
                    _t.SetCoords(x, y);
                    _t.InitTile(data.GetDataFromCoords(x,y));
                    currentGrid[x, y] = _t;

                    posToSpawn.z += (tileYSize + tileSpacing);
                }

                posToSpawn.x += (tileXSize + tileSpacing);
                posToSpawn.z = gridCenter.transform.position.z;
            }

            float XOffset = ((tileXSize + tileSpacing) * (data.columnsCount-1)) / 2;
            float YOffset = ((tileYSize + tileSpacing) * (data.rowsCount-1)) / 2;

            foreach (TileSession ts in currentGrid)
            {
                ts.transform.position = new Vector3(ts.transform.position.x - XOffset, gridCenter.transform.position.y ,ts.transform.position.z - YOffset);
                ts.SpawnEntity();
            }
                
        }
        catch
        {
            Debug.LogError("Unable to load from the data container, it could be corrupted");
        }
    }

    public void ClearRoom()
    {
        foreach (var t in currentGrid.Cast<TileSession>().ToList())
        {
            t.ClearTile();
            Destroy(t.gameObject);
        }

        currentGrid = null;
    }

    public void ControlDoors(bool state)
    {
        var doors = currentGrid.Cast<TileSession>().ToList().Where(t => t.GetTileType() == TileType.Door);

        foreach (var d in doors)
            d.UpdateDoor(state);

        if (state != doorState)
        {
            if (!state)
            {
                PlayClipOnce(doorCloseClip, 1f);
            }
            else
            {
                PlayClipOnce(doorOpenClip, 0.8f);
            }
        }

        doorState = state;
    }

    public void PlayClipOnce(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = false;
        audioSource.Play();
    }
}


