using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/SoundCheck", fileName = "SoundCheckConfig")]
public class SoundCheckConfig : AbilityConfig
{
    public override void Make()
    {
        _ability = new SoundCheckAbility();
        base.Make();
    }
}
