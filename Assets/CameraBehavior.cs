using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior 
    : MonoBehaviour
{
    public float smoothTime = 0.10f;
    public Vector3 offset = Vector3.zero;
    private Vector3 curVelocity = Vector3.zero;

    public Transform target;

    void Update()
    {
        if (target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position - offset, ref curVelocity, smoothTime);
        }
    }
}
