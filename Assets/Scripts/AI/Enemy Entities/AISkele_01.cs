using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkele_01 : AIBase
{
    //vars
    public float maxMoveTime = 0f;
    public float curMoveTime = 0f;
    public float maxIdleTime = 0f;
    public float curIdleTime = 0f;
    public bool isFacingLeft = false;

    //test for ai attacking
    public Rect aggroBox;
    public float aggroDist;
    public Vector2 aggroDir;
    public LayerMask mask;

    //comps
    private Rigidbody2D myRb;

    // Use this for initialization
    void Start()
    {
        //init vars
        curMoveTime = maxMoveTime;
        curIdleTime = maxIdleTime;
        curSpeed = maxSpeed;

        //init comps
        myRb = GetComponent<Rigidbody2D>();
    }

    //Update states
    protected override void UpdateIdle()
    {
        //Find player
        FindPlayer();

        myRb.velocity = new Vector2(0, myRb.velocity.y);

        //Idle time
        curIdleTime -= Time.deltaTime;
        if (curIdleTime < 0f)
        {
            curState = Estate.Emove;
            curIdleTime = maxIdleTime;
        }
    }
    protected override void UpdateMove()
    {
        //Find player
        FindPlayer();

        //Move ai
        if (isFacingLeft)
        {
            //move timer
            curMoveTime -= Time.deltaTime;
            if (curMoveTime < 0f)
            {
                curState = Estate.Eidle;

                //moves the ai the other way next time
                isFacingLeft = false;
                curMoveTime = maxMoveTime;
            }
            else
            {
                //move the ai
                myRb.velocity = new Vector2(-curSpeed, myRb.velocity.y);
            }
        }
        if (!isFacingLeft)
        {
            //move timer
            curMoveTime -= Time.deltaTime;
            if (curMoveTime < 0f)
            {
                curState = Estate.Eidle;

                //moves the ai the other way next time
                isFacingLeft = true;
                curMoveTime = maxMoveTime;
            }
            else
            {
                //move the ai
                myRb.velocity = new Vector2(curSpeed, myRb.velocity.y);
            }

        }
    }
    protected override void UpdateAttack()
    {
        //Nothign
    }

    //Overriding the virtual function
    protected override void FindPlayer()
    {
        base.FindPlayer();

        //Create box cast
        RaycastHit2D hit = Physics2D.BoxCast(
            (Vector2)this.transform.position + aggroBox.center,
            aggroBox.size,
            this.transform.eulerAngles.z,
            aggroDir,
            aggroDist,
            mask);

        //Check for hits
        if (hit.collider)
        {

            curMoveTime = maxMoveTime;
            curIdleTime = maxIdleTime;
            curState = Estate.Eidle;
        }
    }

    //Draw boxcast
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS((Vector2)this.transform.position + aggroBox.center, this.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, aggroBox.size);
        Gizmos.matrix = Matrix4x4.TRS((Vector2)this.transform.position + aggroBox.center + (aggroDir.normalized * aggroDist), this.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, aggroBox.size);
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS((Vector2)this.transform.position + aggroBox.center, Quaternion.identity, Vector3.one);
        Gizmos.DrawLine(Vector2.zero, aggroDir.normalized * aggroDist);
    }
}
