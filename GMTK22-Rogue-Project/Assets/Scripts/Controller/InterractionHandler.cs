using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SelectionModel<T> where T : UnityEngine.Object
{
    [SerializeField, ReadOnly, AllowNesting]
    T currentSelected;

    public T CurrentSelected
    {
        get { return currentSelected; }
        set
        {
            if (value == null)
            {
                if (currentSelected == null)
                    return;
                SelectionChanged?.Invoke(currentSelected,null);
                currentSelected = null;
            }
            else if (value != currentSelected)
            {
                SelectionChanged?.Invoke(currentSelected, value);
                currentSelected = value;
            }
        }
    }

    /// <summary>
    /// first T = previous;
    /// last T = next;
    /// </summary>
    public event Action<T,T> SelectionChanged;

    public void SetSelected(T _obj)
    {
        CurrentSelected = _obj;
    }
}

public class InterractionHandler : MonoBehaviour
{
    [SerializeField, ReadOnly]
    LayerMask currentInterractableMask;
    [SerializeField]
    LayerMask tempTileOnlyMask;

    [Space]

    [SerializeField]
    SelectionModel<TileSession> tileSelection;

    Ray cursorRay;
    RaycastHit cursorRayOut;

    public void Start()
    {
        tileSelection.SelectionChanged += OnTileSelectionChanged;
    }

    private void Update()
    {
        cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cursorRay, out cursorRayOut, Mathf.Infinity, tempTileOnlyMask))
        {
            tileSelection.SetSelected(cursorRayOut.collider.gameObject.GetComponent<TileSession>());
        }
        else
        {
            tileSelection.SetSelected(null);
        }

        if (Input.GetMouseButtonDown(0))
        {
            tileSelection.CurrentSelected?.CallRightClick();
        }
    }

    void OnTileSelectionChanged(TileSession lastTile, TileSession nextTile)
    {
        lastTile?.CallHoverExit();
        nextTile?.CallHoverEnter();
    }
}
