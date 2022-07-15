using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GridGenerator : MonoBehaviour
{
    [SerializeField, ShowIf("ShowTileList")]
    List<Tile> allTiles;
    bool ShowTileList => standaloneMode && allTiles?.Count > 0;

    [Header("Grid Caracs")]
    [SerializeField, Tooltip("Check this to made the script run on his own")]
    bool standaloneMode;

    [SerializeField]
    int rowCount;
    [SerializeField]
    int columnsCount;
    [SerializeField]
    float tileSize;
    [SerializeField]
    float tileSpacing;
    [SerializeField, Tooltip("Point on chich is centered the bottom left tile of the grid")]
    Transform origin;

    [Header("Grid Ressources")]
    [SerializeField]
    Tile tileTemplate;

    private void Start()
    {
        if (standaloneMode)
        {
            allTiles = GenerateGrid();
        }
    }

    public List<Tile> GenerateGrid(Transform parent = null)
    {
        var sizeT = tileTemplate.GetTileSize();
        float tileXSize = sizeT.Item1;
        float tileYSize = sizeT.Item2;
        List<Tile> gridTileList = new List<Tile>();

        Vector3 posToSpawn = origin ? origin.position : Vector3.zero;
        Tile _t;

        if (parent == null) parent = transform;

        for (int x = 0; x < columnsCount; x++)
        {
            posToSpawn.x += (tileXSize + tileSpacing);
            posToSpawn.y = 0;

            for (int y = 0; y < rowCount; y++)
            {
                posToSpawn.y += (tileYSize + tileSpacing);

                _t = Instantiate(tileTemplate, posToSpawn, Quaternion.identity, parent);
                _t.SetCoords(x + 1, y + 1);
                gridTileList.Add(_t);
            }
        }
        return gridTileList;
    }



}
