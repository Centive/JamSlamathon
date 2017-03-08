using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNormal_01 : ProjectileBase
{
    //vars
    public float angularVelocity = 0f;
    
    //comps
    private Rigidbody2D myRb;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        myRb = GetComponent<Rigidbody2D>();
    }

    protected override void FireBullet()
    {
        myRb.velocity = bulletDir * curSpeed;
        myRb.angularVelocity = angularVelocity;
        
        killTime -= Time.deltaTime;
        if(killTime <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}