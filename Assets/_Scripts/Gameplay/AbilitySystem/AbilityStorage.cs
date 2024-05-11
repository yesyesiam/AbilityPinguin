using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AbilityStorage : MonoBehaviour
{
    [SerializeField] private AbilityConfig[] _abilityConfigs;

    [Header("UI References")]
    [SerializeField] private AbilityPanel _abilityPanel;
    [SerializeField] private ConfirmCastPanel _confirmCastPanel;

    private TestOne _testOne;
    private RowManager _rowManager;

    private List<Ability> _abilities = new List<Ability>();
    private Ability _currentAbility;

    private int[] prevCooldownSnapshots;
    private int[] currCooldownSnapshots;

    public Ability[] GetAbilities() => _abilities.ToArray();

    public bool IsCurrentAbilityNotNull => _currentAbility != null;

    public void Init(TestOne testOne, RowManager rowManager)
    {
        _testOne = testOne;
        _rowManager = rowManager;
        for (int i = 0; i < _abilityConfigs.Length; ++i)
        {
            _abilityConfigs[i].Make();

            var ability = _abilityConfigs[i].GetResult();
            ability.SetReferences(_rowManager, _testOne);
            _abilities.Add(ability);
        }

        _abilityPanel.Init(GetAbilities());
        _confirmCastPanel.Init();

        _abilityPanel.OnClickAbility += OnClickAbilityButton;
        _confirmCastPanel.onApplyButtonClicked.AddListener(ApplyCast);
        _confirmCastPanel.onCancelButtonClicked.AddListener(CancelCast);

        InitCD();
    }

    /*private void Update()
    {
        if (_currentAbility != null)
        {
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.TryGetComponent<Platform>(out var platform))
                    {
                        Debug.Log(platform.index);
                        SetTarget(platform);
                        _testOne.SetTarget(platform);
                    }
                }
            }
        }
    }*/

    private void OnDestroy() 
    {
        _abilityPanel.OnClickAbility -= OnClickAbilityButton;
        _confirmCastPanel.onApplyButtonClicked.RemoveListener(ApplyCast);
        _confirmCastPanel.onCancelButtonClicked.RemoveListener(CancelCast);
    }

    public void OnClickAbilityButton(int abilityIndex)
    {
        if (_currentAbility == null && _abilities[abilityIndex].IsReady
            && (_testOne.GetStateForAbility()==1 || (_abilities[abilityIndex].CanCastWhenFall && _testOne.GetStateForAbility() == 0)) )
        {
            Debug.Log("start cast ability: " + abilityIndex);
            _currentAbility = _abilities[abilityIndex];
            var canCast = _currentAbility.CheckCondition();
            _confirmCastPanel.ShowPanel(canCast);
        }
    }

    public void SetTarget(Platform p)
    {
        var canCast = _currentAbility.CheckCondition(p);
        _confirmCastPanel.ShowPanel(canCast);
    }

    public void ApplyCast()
    {
        _currentAbility.ApplyCast();
        CancelCast();
    }

    public void CancelCast()
    {
        _currentAbility = null;
        _testOne.CancelCast();
    }

    public void ReduceCD()
    {
        foreach (var ability in _abilities)
        {
            ability.ReduceCooldown();
        }
    }

    public void TakeSnapshot()
    {
        prevCooldownSnapshots = currCooldownSnapshots;
        currCooldownSnapshots = new int[_abilities.Count];
        for (int i = 0; i < _abilities.Count; i++)
        {
            currCooldownSnapshots[i] = _abilities[i].CooldownCount;
        }
    }

    public void BackOneTurn()
    {

        for (int i = 0; i < prevCooldownSnapshots.Length; i++)
        {
            _abilities[i].ChangeCooldownCount(prevCooldownSnapshots[i]);
        }
    }

    public void InitCD()
    {
        foreach (var ability in _abilities)
        {
            ability.ChangeCooldownCount(0);
        }

        TakeSnapshot();
    }
}
