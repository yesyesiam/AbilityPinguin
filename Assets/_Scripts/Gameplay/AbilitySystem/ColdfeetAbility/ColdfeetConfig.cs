using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Coldfeet", fileName = "ColdfeetConfig")]
public class ColdfeetConfig : AbilityConfig
{
    [field: SerializeField] public GameObject FreezedPlatformPrefab { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
    public override void Make()
    {
        _ability = new ColdfeetAbility(FreezedPlatformPrefab, Duration);
        base.Make();
    }
}
