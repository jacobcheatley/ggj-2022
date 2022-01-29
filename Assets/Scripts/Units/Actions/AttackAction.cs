﻿using System;
using UnityEngine;


public class AttackAction : IAction
{
    public override void PerformAction(GridManager gridManager, Vector3Int fromCell, Vector3Int toCell)
    {
        base.PerformAction(gridManager, fromCell, toCell);

        GridObject other = gridManager.GetAtPosition(toCell);
        other?.ApplyDamage(value);
    }
}