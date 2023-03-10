using System;

using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] Animator unitAnimator;
    float smallDis = 0.1f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] int max_Move_Dis = 3;
    [SerializeField] int moveActionPointCost = 1;


    Vector3 targetPos;
    GridPosition unitGridPosition;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;


    protected override void Awake()
    {
        base.Awake();
        targetPos = transform.position;

    }

    void Update()
    {
        if (!isActive) return;
        Move();
    }

    private void Move()
    {

        Vector3 unitDirection = (targetPos - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPos) > smallDis)
        {
            //Direction and normalized distance
            transform.position += unitDirection * moveSpeed * Time.deltaTime;
            //notice this lerp is not linear since transform.forward is chaning if linear is needed it need to be stored every change in direction.
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {

            OnStopMoving?.Invoke(this, EventArgs.Empty);

            ActionComplete();

        }
        transform.forward = Vector3.Lerp(transform.forward, unitDirection, Time.deltaTime * rotateSpeed);

    }

    public override void TakeAction(GridPosition gridPosition, Action onMoveActionComplete)
    {
        //if targetPos is different from transform.position will trigger the Move();
        this.targetPos = LevelGrid.Instance.GetWorldPositionFromGridPos(gridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onMoveActionComplete);




    }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetUnitGridPosition();
        for (int x = -max_Move_Dis; x <= max_Move_Dis; x++)
        {
            for (int z = -max_Move_Dis; z <= max_Move_Dis; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;
                if (testGridPosition == unitGridPosition) continue;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (LevelGrid.Instance.HasUnitAtGridPosition(testGridPosition)) continue;
                validGridPositions.Add(testGridPosition);

                //Debug.Log("x is " + x + "z is " + z + "Valid grid is " + testGridPosition.ToString());

            }
        }

        return validGridPositions;
    }




    public override string GetActionName()
    {
        return "Move";
    }

    public override int GetActionPointsCost()
    {
        return moveActionPointCost;
    }

    public override EnemyActionValue GenerateEnemyActionValue(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetShootAction().GetTargetCountAtGridPosition(gridPosition);

        return new EnemyActionValue
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

}
