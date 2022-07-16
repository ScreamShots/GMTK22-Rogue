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
                    _t = Instantiate(tileSessionTemplate, posToSpawn, Quaternion.identity, gridParent);
                    _t.SetCoords(x, y);
                    _t.InitTile(data.GetDataFromCoords(x,y));
                    currentGrid[x, y] = _t;

                    posToSpawn.y += (tileYSize + tileSpacing);
                }

                posToSpawn.x += (tileXSize + tileSpacing);
                posToSpawn.y = 0;
            }

            float XOffset = ((tileXSize + tileSpacing) * (data.columnsCount-1)) / 2;
            float YOffset = ((tileYSize + tileSpacing) * (data.rowsCount-1)) / 2;

            foreach (TileSession ts in currentGrid)
                ts.transform.position = new Vector3(ts.transform.position.x - XOffset, ts.transform.position.y - YOffset, 0);
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
            Destroy(t);
        }

        currentGrid = null;
    }
}


