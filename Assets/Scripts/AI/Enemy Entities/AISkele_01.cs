using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkele_01 : AIBase
{
    public float maxMoveTime;
    public float curMoveTime;
    public float maxIdleTime;
    public float curIdleTime;
    public bool isFacingLeft = false;

    //my components
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

    }
}
