using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeControl : MonoBehaviour
{

    public Action OnResetTargets;
    
    public void ResetTargets()
    {
        Debug.Log("Reset targets");
        OnResetTargets?.Invoke();
        Zombie[] zombies = FindObjectsByType<Zombie>(FindObjectsSortMode.None);
        foreach (Zombie zombie in zombies)
        {
            Destroy(zombie.transform.gameObject);
        }
        
        Navigation[] navs = FindObjectsByType<Navigation>(FindObjectsSortMode.None);
        foreach (Navigation nav in navs)
        {
            Destroy(nav.transform.gameObject);
        }
    }
}
