using UnityEngine;

public abstract class AbilityConfig : ScriptableObject
{
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite DisplayImage { get; private set; }
    [field: SerializeField] public int CooldownTotal { get; private set; }
    [field: SerializeField] public int CastDistance { get; private set; }

    [field: SerializeField] public bool CanCastWhenFall { get; private set; }

    //public virtual AbilityBuilder GetBuilder() => new AbilityBuilder(this);
    protected Ability _ability;

    public virtual void Make() {

        if (_ability != null)
        {
            _ability.SetDescription(Title, Description, DisplayImage);
            _ability.SetCooldown(CooldownTotal);
            _ability.CastDistance = CastDistance;
            _ability.CanCastWhenFall = CanCastWhenFall;
        }
    }

    public virtual Ability GetResult() => _ability;
}
