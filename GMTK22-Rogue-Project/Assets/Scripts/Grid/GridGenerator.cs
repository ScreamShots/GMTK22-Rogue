using UnityEngine;

[System.Serializable]
public class GridParams
{
    public int rowCount;
    public int columnsCount;
    [Tooltip("Point on which is centered the bottom left tile of the grid")]
    public Transform origin;
    public float tileSize;
    public float tileSpacing;

    public GridParams(int _rowCount, int _columnsCount, Transform _origin = null, float _tileSize = 1f, float _tileSpacing = 0.2f)
    {
        rowCount = _rowCount;
        columnsCount = _columnsCount;
        origin = _origin;
        tileSize = _tileSize;
        tileSpacing = _tileSpacing;
    }
}

[System.Serializable]
public class GridGenerator
{
    [SerializeField]
    GridParams defaultParams;

    [Header("Grid Ressources")]
    [SerializeField]
    Tile tileTemplate;

    public Tile[,] GenerateGrid(Transform parent, GridParams _gridParams = null)
    {
        if (_gridParams == null)
            _gridParams = defaultParams;

        var sizeT = tileTemplate.GetTileSize();
        float tileXSize = sizeT.Item1;
        float tileYSize = sizeT.Item2;
        Tile[,] gridTileList = new Tile[_gridParams.columnsCount,_gridParams.rowCount];

        Vector3 posToSpawn = _gridParams.origin ? _gridParams.origin.position : Vector3.zero;
        Tile _t;

        for (int x = 0; x < _gridParams.columnsCount; x++)
        {
            for (int y = 0; y < _gridParams.rowCount; y++)
            {
                _t = Object.Instantiate(tileTemplate, posToSpawn, Quaternion.identity, parent);
                _t.SetCoords(x, y);
                gridTileList[x,y] = _t;

                posToSpawn.y += (tileYSize + _gridParams.tileSpacing);
            }

            posToSpawn.x += (tileXSize + _gridParams.tileSpacing);
            posToSpawn.y = 0;
        }
        return gridTileList;
    }
}
