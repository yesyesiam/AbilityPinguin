using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int row = -1;
    public int index = -1;

    public bool IsBreakable = false;

    public bool IsAlive { get; private set; } = true;

    public void Break()
    {
        IsAlive = false;
        gameObject.SetActive(IsAlive);
    }

    public void Setup(bool isAlive)
    {
        IsAlive = isAlive;
        gameObject.SetActive(IsAlive);
    }

    public bool PerformPlatformAction()
    {
        if (pType == PType.Default && IsAlive && IsBreakable)
        {
            Break();
            return true;
        }
        return false;
    }

    public FreezeDebuff buff { get; private set; }

    public bool IsBuffed => buff != null;


    public void AddBuff(FreezeDebuff debuff)
    {
        if (buff == null)
        {
            buff = debuff;
        } 
    }
    public void RemoveBuff()
    {
        if (buff != null)
        {
            buff.Clear();
            buff = null;
        }
    }
    
    public PType pType;
    public enum PType
    {
        Default,
        OneWay
    }

    private void OnDestroy()
    {
        RemoveBuff();
    }
}
