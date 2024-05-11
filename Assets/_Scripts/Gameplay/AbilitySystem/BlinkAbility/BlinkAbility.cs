using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAbility : Ability
{
    private Platform _targetPlatform;
    public override bool CheckCondition(Platform platform)
    {
        if (platform != null && CanCastAbility(platform.row))
        {
            _targetPlatform = platform;
            return true;
        }
        _targetPlatform = null;
        return false;
    }
    public override void ApplyCast()
    {
        Debug.Log("suka blink.");
        RowManager.TpToPlatform(_targetPlatform);
        var toPos = RowManager.TargetPointPosition;
        toPos.y = TestOne.transform.position.y;

        TestOne.transform.position = toPos;
        TestOne.ApplyMove();

        ChangeCooldownCount(CooldownTotal);
    }
}
