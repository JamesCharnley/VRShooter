using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeControlButton : MonoBehaviour, IInteractable
{
    [SerializeField] protected ShootingRangeControl control;
    // Start is called before the first frame update
    void Start()
    {
        ConnectButtonToControl();
    }

    protected virtual void ConnectButtonToControl()
    {
        
    }
    
    protected virtual void DisconnectButtonToControl()
    {
        
    }

    public virtual void Interact()
    {
        
    }

    private void OnDestroy()
    {
        DisconnectButtonToControl();
    }
}
