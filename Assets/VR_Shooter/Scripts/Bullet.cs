using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float startForce;

    private Vector3 prevPosition;

    [SerializeField] private GameObject soundPrefab;

    [SerializeField] private float hitForce = 10;

    [SerializeField] private float damage = 20;

    private Vector3 startPosition;

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i > -1; i--)
            {
                transform.GetChild(i).transform.SetParent(null);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        prevPosition = transform.position;
        rb.AddForce(transform.forward * startForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForCollision();

        prevPosition = transform.position;
    }

    void CheckForCollision()
    {
        float dist = Vector3.Distance(prevPosition, transform.position);
        if (Physics.Raycast(prevPosition, transform.position - prevPosition, out RaycastHit hit, dist))
        {
            Debug.DrawLine(startPosition, hit.point, Color.green, 5);
            if(hit.transform.gameObject.TryGetComponent(out ITakeDamage takeDamage))
            {
                takeDamage.TakeDamage(damage);
            }

            if (hit.transform.TryGetComponent(out RagdollLimb ragLimb))
            {
                ragLimb.ApplyForce((transform.position - prevPosition).normalized * hitForce, ForceMode.Impulse, hit.point);
            }
            else
            {
                if (hit.rigidbody is not null)
                {
                    hit.rigidbody.AddForceAtPosition((transform.position - prevPosition).normalized * hitForce,hit.point, ForceMode.Impulse);
                }
            }

            if (hit.transform.TryGetComponent(out Surface surface))
            {
                if (soundPrefab)
                {
                    GameObject impSound = Instantiate(soundPrefab, hit.point, quaternion.identity);
                    if (impSound.transform.TryGetComponent(out AudioSource source))
                    {
                        source.clip = SoundDirectory.instance.GetBulletImpactSound(surface.GetSurfaceType());
                        source.Play();
                    }
                }
                
            }
            Destroy(gameObject);
        }
    }
}
