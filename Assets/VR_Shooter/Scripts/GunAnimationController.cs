using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationController : MonoBehaviour
{
    [SerializeField] protected Gun gun;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        gun.OnShoot += PlayShootAnimation;
        anim = GetComponent<Animator>();
    }

    private void PlayShootAnimation()
    {
        Debug.Log("Shoot");
        anim.SetTrigger("Shoot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        gun.OnShoot -= PlayShootAnimation;
    }
}
