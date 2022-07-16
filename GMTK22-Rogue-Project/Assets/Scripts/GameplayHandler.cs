using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameplayHandler : MonoBehaviour
{
    [SerializeField]
    GridGenerator gridGenerator;
    Tile[,] currentGrid;

    [Button("Generate Default Grid")]
    public void TestGenerateGrid() 
    {
        if(currentGrid != null)
            foreach (Tile t in currentGrid.Cast<Tile>().ToList())
                Destroy(t.gameObject);

        currentGrid = gridGenerator.GenerateGrid(transform); 
    }
}


