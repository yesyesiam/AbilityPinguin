using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdfeetAbility : Ability
{
    private GameObject vfx;
    private float duration;
    public ColdfeetAbility(GameObject VFX, float Duration)
    {
        vfx = VFX;
        duration = Duration;
    }
    private Platform _targetPlatform;

    public override bool CheckCondition(Platform platform)
    {
        if (platform != null 
            && platform.pType == Platform.PType.Default 
            && CanCastAbility(platform.row)
            && !platform.IsBuffed)
        {
            _targetPlatform = platform;
            return true;
        }
        _targetPlatform = null;
        return false;
    }
    public override void ApplyCast()
    {
        Debug.Log("suka cold.");
        var debuff = new FreezeDebuff();

        debuff.Init(_targetPlatform, RowManager, vfx, duration);
        _targetPlatform.AddBuff(debuff);

        ChangeCooldownCount(CooldownTotal);
    }
}
