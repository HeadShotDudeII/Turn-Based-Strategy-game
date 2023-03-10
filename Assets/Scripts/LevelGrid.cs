using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform debugPrefab;
    private GridSystem gridSystem;
    public event EventHandler OnAnyUnitMovedPosition;
    public static LevelGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) // should be null before initialzed
        {
            Destroy(gameObject);

        }
        Instance = this;

        gridSystem = new GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObjects(debugPrefab);
    }

    private void Start()
    {
        gridSystem.DisplayGridDebugObjects();

    }

    private void Update()
    {
        gridSystem.DisplayGridDebugObjects();
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);

    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public GridPosition GetGridPositionFromWorldPos(Vector3 worldPos)
    {
        return gridSystem.GetGridPositionFromWorldPos(worldPos);
    } // Best not to expose GridSystem because LevelGrid is made up of GridSystem.

    //public GridPosition GetGridPositionFromWorldPos(Vector3 worldPos) => gridSystem.GetGridPositionFromWorldPos(worldPos);

    public Vector3 GetWorldPositionFromGridPos(GridPosition gridPosition) => gridSystem.GetWorldPositionFromGridPos(gridPosition);

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        Debug.Log("Removed from " + fromGridPosition.ToString() + "Added to " + toGridPosition);
        OnAnyUnitMovedPosition?.Invoke(this, EventArgs.Empty);

    }

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool HasUnitAtGridPosition(GridPosition gridPosition) => gridSystem.HasUnitAtGridPosition(gridPosition);

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) => gridSystem.GetUnitAtGridPosition(gridPosition);

    public int GetHeight() => gridSystem.GetHeight();

    public int GetWidth() => gridSystem.GetWidth();



}
