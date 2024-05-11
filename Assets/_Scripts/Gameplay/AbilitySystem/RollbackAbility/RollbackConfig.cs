using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Rollback", fileName = "RollbackConfig")]
public class RollbackConfig : AbilityConfig
{
    public override void Make()
    {
        _ability = new RollbackAbility();
        base.Make();
    }
}
