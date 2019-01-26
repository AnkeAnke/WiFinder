using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Transform Target;
    public float Speed = 5.0f;
    
    private Vector3 startPos;
    private Vector3 toTarget;
    private float animationTime;
    private float lastDirSwapTime;
    private bool backwards = false;
   
    void Start()
    {
        startPos = transform.position;
        toTarget = Target.transform.position - startPos;
        lastDirSwapTime = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
        float dt = Time.time - lastDirSwapTime;
        animationTime = toTarget.magnitude / Speed;
        if (backwards)
            transform.position = startPos + toTarget * (1.0f - dt / animationTime);
        else
            transform.position = startPos + toTarget * (dt / animationTime);
        if (dt > animationTime)
        {
            lastDirSwapTime = Time.time;
            backwards = !backwards;
        }
    }
}
