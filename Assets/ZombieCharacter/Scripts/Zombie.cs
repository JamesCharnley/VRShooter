using System.Collections;
using System.Collections.Generic;
using FIMSpace.FProceduralAnimation;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Unity.Entities;
using UnityEngine;

public class Zombie : MonoBehaviour, ITakeDamage
{
    [SerializeField] private float currentHealth = 100;
    public bool isDead = false;
    private RagdollAnimator ragdoll;

    private AnimationController animController;
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<RagdollAnimator>();
        animController = GetComponent<AnimationController>();
        //StartCoroutine(WaitDie());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitDie()
    {
        yield return new WaitForSeconds(2);
        Die();
    }
    public void TakeDamage(float _amount)
    {
        if (isDead) return;
        currentHealth -= _amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        GameObject agentObject = transform.parent.gameObject;
        transform.SetParent(null);
        
        //AgentCylinderShapeAuthoring cyl = agentObject.GetComponent<AgentCylinderShapeAuthoring>();
        //AgentAuthoring agent = agentObject.GetComponent<AgentAuthoring>();
        //AgentColliderAuthoring col = agentObject.GetComponent<AgentColliderAuthoring>();
        //AgentSeparationAuthoring sep = agentObject.GetComponent<AgentSeparationAuthoring>();
        //AgentAvoidAuthoring avoid = agentObject.GetComponent<AgentAvoidAuthoring>();
        //AgentNavMeshAuthoring nav = agentObject.GetComponent<AgentNavMeshAuthoring>();
        //Destroy(nav);
        //Destroy(avoid);
        //Destroy(sep);
        //Destroy(cyl);
        //Destroy(col);
        //Destroy(agent);
        //Destroy(agentObject);
        
        
        Destroy(agentObject);
        ragdoll.User_FadeRagdolledBlend(1);
        ragdoll.User_FadeMuscles(0);
        ragdoll.User_SwitchFreeFallRagdoll(true);
        ragdoll.User_SetVelocityAll(animController.agentVelocity);
    }
}
