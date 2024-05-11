using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Blink", fileName = "BlinkConfig")]
public class BlinkConfig : AbilityConfig
{
    public override void Make()
    {
        _ability = new BlinkAbility();
        base.Make();
    }
}
