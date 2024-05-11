using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollbackAbility : Ability
{
    public override bool CheckCondition(Platform platform)
    {
        return RowManager.CanRollback;
    }
    public override void ApplyCast()
    {
        //Storage.BackOneTurn();

        //RowManager.SetupByShapshot();
        TestOne.Rollback();

        ChangeCooldownCount(CooldownTotal);
    }
}
