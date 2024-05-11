using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public event Action<int, int> EventChangeCooldownTimer;
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Sprite DisplayImage { get; private set; }

    public int CooldownTotal { get; private set; }
    public int CooldownCount { get; private set; }
    public bool CanCastWhenFall { get; set; }
    public bool IsReady => CooldownCount == 0;

    public RowManager RowManager { get; private set; }
    public TestOne TestOne { get; private set; }

    private int castDistance;

    public int CastDistance
    {
        get { return castDistance; }
        set { castDistance = value >= 0 ? value : 1; }
    }


    public void SetDescription(string title, string description, Sprite displayImage)
    {
        Title = title;
        Description = description;
        DisplayImage = displayImage;
    }

    public void SetCooldown(int total)
    {
        CooldownTotal = total;
    }

    public void SetReferences(RowManager rowManager, TestOne testOne) 
    {
        RowManager = rowManager;
        TestOne = testOne;
    }

    public virtual void ReduceCooldown()
    {
        if (CooldownCount > 0)
        {
            ChangeCooldownCount(CooldownCount - 1);
        }
    }

    public void ChangeCooldownCount(int count)
    {
        CooldownCount = count;
        EventChangeCooldownTimer?.Invoke(CooldownCount, CooldownTotal);
    }

    public virtual bool CheckCondition(Platform platform=null) => true;
    public virtual void ApplyCast() { }
    public virtual void EventTick(float deltaTick) { }

    public virtual bool CanCastAbility(int targetIndex)
    {
        int characterIndex = RowManager.CurrentRow;

        if (Mathf.Abs(targetIndex - characterIndex) > castDistance)
        {
            return false;
        }

        return targetIndex > characterIndex;
    }
}
