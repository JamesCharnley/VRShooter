using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTargetsButton : ShootingRangeControlButton
{
    

    public override void Interact()
    {
        control.ResetTargets();
    }
}
