using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FreezeDebuff
{
    public event Action<string> EventChangeTimer;

    private Platform owner;
    private RowManager manager;
    private GameObject vfxObject;
    public float Duration { get; set; } = 3;

    private Coroutine debuffCoroutine;
    private bool oldBrekableState;
    private bool showTimer = false;
    public void Init(Platform platform, RowManager rmanager, GameObject vfx, float duration=3)
    {
        owner = platform;
        manager = rmanager;
        Duration = duration;

        vfxObject = GameObject.Instantiate(vfx, owner.transform.position, Quaternion.identity);

        oldBrekableState = platform.IsBreakable;
        platform.IsBreakable = false;
        manager.OnPlatformChanged += Manager_OnPlatformChanged;
    }

    private void Manager_OnPlatformChanged(Platform newPlatform)
    {
        if (newPlatform == owner && debuffCoroutine == null)
        {
            showTimer = true;
            debuffCoroutine = manager.StartCoroutine(CancelDebuff());
        } 
        else
        {
            showTimer = false;
        }
    }

    private IEnumerator CancelDebuff()
    {
        float timer = Duration;
        float tickTime = 0.1f;
        WaitForSeconds waitForIt = new WaitForSeconds(tickTime);
        string s;
        while (timer > 0f)
        {
            s = showTimer ? timer.ToString("F1", System.Globalization.CultureInfo.InvariantCulture) : string.Empty;
            EventChangeTimer?.Invoke(s);
            
            yield return waitForIt;

            timer -= tickTime;
        }

        owner.RemoveBuff();
        Debug.Log("Debuff ended!");
    }

    public void Clear()
    {
        if (debuffCoroutine != null)
        {
            manager.StopCoroutine(debuffCoroutine);
            debuffCoroutine = null;
        }
       
        manager.OnPlatformChanged -= Manager_OnPlatformChanged;
        GameObject.Destroy(vfxObject);

        if (owner != null)
        {
            owner.IsBreakable = oldBrekableState;

            manager.UpdatePlatform(owner);
        }
        EventChangeTimer?.Invoke(String.Empty);
        Debug.Log("bindings cleared");
    }
}
