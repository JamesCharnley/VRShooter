using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiAutomaticGun : Gun, IAnimateController
{
    [SerializeField] private string triggerAnimation;
    public string TriggerAnimation
    {
        get => triggerAnimation;
        set => triggerAnimation = value;
    }
}
