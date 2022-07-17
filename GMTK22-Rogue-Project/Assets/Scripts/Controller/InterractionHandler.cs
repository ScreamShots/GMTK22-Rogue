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
                SelectionChanged?.Invoke(currentSelected, null);
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
    public event Action<T, T> SelectionChanged;

    public void SetSelected(T _obj)
    {
        CurrentSelected = _obj;
    }

    public void Clear() => currentSelected = null;
}

public class InterractionHandler : MonoBehaviour
{
    public static Vector3 MousePosWorld;

    [SerializeField]
    LayerMask tileLayerMask;
    [SerializeField]
    LayerMask diceLayerMask;

    [Space]

    [SerializeField]
    SelectionModel<TileSession> tileSelection;
    [SerializeField]
    SelectionModel<Dice> diceSelection;

    Ray cursorRay;
    RaycastHit cursorRayOut;

    public void Start()
    {
        tileSelection.SelectionChanged += OnTileSelectionChanged;
        diceSelection.SelectionChanged += OnDiceSelectionChanged;
    }

    public void ResetSelectionModels()
    {
        tileSelection.Clear();
    }

    private void Update()
    {
        cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(cursorRay, out cursorRayOut, Mathf.Infinity, tileLayerMask))
        {
            tileSelection.SetSelected(cursorRayOut.collider.gameObject.GetComponent<TileSession>());
        }
        else
        {
            tileSelection.SetSelected(null);
        }

        if (Physics.Raycast(cursorRay, out cursorRayOut, Mathf.Infinity,diceLayerMask))
        {
            if (!(diceSelection.CurrentSelected?.currentState == States.Moved && Input.GetMouseButton(0)))
                diceSelection.SetSelected(cursorRayOut.collider.gameObject.GetComponent<Dice>());
        }
        else
        {
            if (!(diceSelection.CurrentSelected?.currentState == States.Moved && Input.GetMouseButton(0)))
                diceSelection.SetSelected(null);
        }

        if (Input.GetMouseButtonDown(0))
            OnRightClick();

        if (Input.GetMouseButton(0))
        {
            if (diceSelection.CurrentSelected?.currentState != States.Moved)
                diceSelection.CurrentSelected?.ChangeState(States.Moved);

            var selectedDice = diceSelection.CurrentSelected;
            if (selectedDice != null)
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedDice.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedDice.transform.position = new Vector3(worldPosition.x, 2f, worldPosition.z);
            }

        }
        else
        {
            if (diceSelection.CurrentSelected?.currentState == States.Moved)
                diceSelection.CurrentSelected?.ChangeState(States.Display);
        }
    }

    void OnTileSelectionChanged(TileSession lastTile, TileSession nextTile)
    {
        lastTile?.CallHoverExit();
        nextTile?.CallHoverEnter();
    }

    void OnDiceSelectionChanged(Dice lastDice, Dice nextDice)
    {
        if (lastDice?.currentState == States.Moved)
        {
            lastDice.ChangeState(States.Display);
        }

        if (nextDice != null)
        {
            if (!nextDice.alreadyReoriented)
                nextDice.ReOrientate();
        }
    }

    public void OnRightClick()
    {
        tileSelection.CurrentSelected?.CallRightClick();
    }
}
