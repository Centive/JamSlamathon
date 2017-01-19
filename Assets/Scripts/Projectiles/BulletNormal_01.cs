using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNormal_01 : MonoBehaviour
{
    //vars
    public float bulletSpeed = 0f;
    public float angularVelocity = 0f;
    public Vector2 bulletDir = Vector2.right;

    //comps
    private Rigidbody2D myRb;

    // Use this for initialization
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRb.velocity = bulletDir * bulletSpeed;
        myRb.angularVelocity = angularVelocity;
    }
}