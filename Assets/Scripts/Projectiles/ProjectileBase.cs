using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public float maxSpeed = 0f;
    public float curSpeed = 0f;
    public float killTime = 0f;
    public Vector3 bulletDir = Vector3.zero;

    protected abstract void FireBullet();

    protected virtual void Start()
    {
        curSpeed = maxSpeed;
    }
    
    // Update is called once per frame
    void Update()
    {
        FireBullet();
    }
}
