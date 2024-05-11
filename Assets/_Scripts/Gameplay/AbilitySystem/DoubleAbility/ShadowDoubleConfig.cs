using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ShadowDouble", fileName = "ShadowDoubleConfig")]
public class ShadowDoubleConfig : AbilityConfig
{
    [field: SerializeField] public GameObject VFX { get; private set; }
    public override void Make()
    {
        _ability = new ShadowDoubleAbility(VFX);
        base.Make();
    }
}
