using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior 
    : MonoBehaviour
{
    //start singleton
    private static CameraBehavior instance;
    public static CameraBehavior Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    //end singleton

    public float smoothTime = 0.10f;
    public Vector3 offset = Vector3.zero;
    public Transform target;
    
    void FixedUpdate()
    {
        CameraFollow();
    }
    
    //Override this function if to create a different follow code
    public virtual void CameraFollow()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position - offset, target.position, smoothTime * Time.deltaTime);
        }
    }
}