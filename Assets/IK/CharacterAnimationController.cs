using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private Transform camera;

    [SerializeField] private float minCameraDistance = 1;
    [SerializeField] private float maxCameraDistance = 1;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCrouch();
    }

    void UpdateCrouch()
    {
        float dist = Vector3.Distance(transform.position + Vector3.up * minCameraDistance, camera.transform.position);

        if (dist == 0)
        {
            anim.SetFloat("Height", 0);
        }
        else
        {
            float ratio = dist / (maxCameraDistance - minCameraDistance);
        
            anim.SetFloat("Height", ratio);
        }
        
    }
}
