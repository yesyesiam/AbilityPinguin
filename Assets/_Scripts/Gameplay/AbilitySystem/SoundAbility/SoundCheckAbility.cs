using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCheckAbility : Ability
{
    private Platform _targetPlatform;
    public override bool CheckCondition(Platform platform)
    {
        if (platform != null && platform.pType == Platform.PType.Default && CanCastAbility(platform.row))
        {
            _targetPlatform = platform;
            return true;
        }
        _targetPlatform = null;
        return false;
    }
    public override void ApplyCast()
    {
        Debug.Log("check by sound.");
        RowManager.PlaySound(_targetPlatform);

        ChangeCooldownCount(CooldownTotal);
    }
}
