using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public enum Estate
    {
        Eidle,
        Emove,
        Eattack
    }

    public Estate curState = Estate.Eidle;

    //my vars
    public float maxSpeed;
    public float curSpeed;
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

    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case Estate.Eidle:
                {
                    UpdateIdle();
                    break;
                }
            case Estate.Emove:
                {
                    UpdateMove();
                    break;
                }
            case Estate.Eattack:
                {
                    UpdateAttack();
                    break;
                }
        }
    }

    //Update states
    void UpdateIdle()
    {
        //Find player
        FindPlayer();

        //Idle time
        curIdleTime -= Time.deltaTime;
        if (curIdleTime < 0f)
        {
            curState = Estate.Emove;
            curIdleTime = maxIdleTime;
        }
    }
    void UpdateMove()
    {
        if(isFacingLeft)
        {
            //move timer
            curMoveTime -= Time.deltaTime;
            if(curMoveTime < 0f)
            {
                curState = Estate.Eidle;
                myRb.velocity = new Vector2(0, myRb.velocity.y);

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

        if(!isFacingLeft)
        {
            //move timer
            curMoveTime -= Time.deltaTime;
            if (curMoveTime < 0f)
            {
                curState = Estate.Eidle;
                myRb.velocity = new Vector2(0, myRb.velocity.y);

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
    void UpdateAttack()
    {

    }

    void FindPlayer()
    {
        //nothing
    }
}
