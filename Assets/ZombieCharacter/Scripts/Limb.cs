using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour, ITakeDamage
{
    [SerializeField] private GameObject ownerGameObject;
    private ITakeDamage owner;
    [SerializeField] private float damageMultiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        owner = ownerGameObject.GetComponent<ITakeDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float _amount)
    {
        if (owner != null)
        {
            owner.TakeDamage(_amount * damageMultiplier);
        }
    }
}
