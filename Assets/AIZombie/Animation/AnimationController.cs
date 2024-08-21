using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation.Hybrid;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private AgentAuthoring agentAuth;
    private Vector3 agentLocalVelocity;
    public Vector3 agentVelocity;
    public Action OnHitLegs;
    private Zombie zombie;

    private float runSpeed = 3;
    private float walkSpeed = 1.5f;
    private float crawlSpeed = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<Zombie>();
        agentAuth = transform.parent.GetComponent<AgentAuthoring>();
        anim = GetComponent<Animator>();
        anim.SetLayerWeight(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (zombie.isDead) return;
        agentVelocity = agentAuth.EntityBody.Velocity;
        agentLocalVelocity = transform.InverseTransformDirection(agentVelocity);
        anim.SetFloat("VelocityX", agentLocalVelocity.x);
        anim.SetFloat("VelocityY", agentLocalVelocity.y);
        anim.SetFloat("VelocityZ", agentLocalVelocity.z);
        anim.SetFloat("Magnitude",Mathf.InverseLerp(0, runSpeed, agentLocalVelocity.magnitude));

        if (hitCooldownTimer > 0)
        {
            hitCooldownTimer -= Time.deltaTime;
        }
    }

    public float GetRunSpeed()
    {
        return runSpeed;
    }

    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public float GetCrawlSpeed()
    {
        return crawlSpeed;
    }

    private float hitCooldown = 1;
    private float hitCooldownTimer = 0;
    public void Hit(Vector3 _localHitPosition)
    {
        if (hitCooldownTimer > 0) return;
        hitCooldownTimer = hitCooldown;
        
        
        if (_localHitPosition.y > 1.4f)
        {
            anim.SetLayerWeight(1, 1);
            anim.CrossFade("HitHead", 0, 1, 0.2f);
            return;
        }

        if (_localHitPosition.y > 1.2f && _localHitPosition.x < 0)
        {
            anim.SetLayerWeight(1, 1);
            anim.CrossFade("HitLeft", 0, 1, 0.2f);
            return;
        }
        
        if (_localHitPosition.y > 1.2f && _localHitPosition.x > 0)
        {
            anim.SetLayerWeight(1, 1);
            anim.CrossFade("HitRight", 0, 1, 0.2f);
            return;
        }

        if (_localHitPosition.y <= 0.9f && _localHitPosition.x > 0)
        {
            anim.SetLayerWeight(1, 0);
            // hit right leg
            anim.CrossFade("HitRightLeg", 0, 0, 0.2f);
            OnHitLegs?.Invoke();
            return;
        }
        if (_localHitPosition.y <= 0.9f && _localHitPosition.x < 0)
        {
            anim.SetLayerWeight(1, 0);
            // hit left leg
            anim.CrossFade("HitLeftLeg", 0, 0, 0.2f);
            OnHitLegs?.Invoke();
            return;
        }
        anim.SetLayerWeight(1, 1);
        anim.CrossFade("HitMiddle", 0, 1, 0.0f);
    }
    public bool CrossFadeAnimation(string _statename, int _layer, float _transitionTime, float _timeOffset, float _layerWeight, bool _overrideOtherLayers)
    {

        if (anim.GetCurrentAnimatorStateInfo(_layer).IsName(_statename)) return false;
        anim.SetLayerWeight(_layer, _layerWeight);
        if (_overrideOtherLayers)
        {
            for (int i = 0; i < anim.layerCount; i++)
            {
                if (i != _layer)
                {
                    anim.SetLayerWeight(i, 0);
                }
            }
        }
        anim.CrossFade(_statename, _transitionTime, _layer, _timeOffset);
        return true;
    }

    public void ResetLayerWeightsToDefault()
    {
        anim.SetLayerWeight(1, 1);
    }

    public void SetNoLegs(bool _value)
    {
        anim.SetBool("NoLegs", true);
    }
    
    public void DisableUpperbodyLayer()
    {
        anim.SetLayerWeight(1, 0);
    }

    public void SetBool(string _name, bool _value)
    {
        anim.SetBool("IsDead", _value);
    }

    public void SetTrigger(string _name)
    {
        anim.SetTrigger(_name);
    }
}
