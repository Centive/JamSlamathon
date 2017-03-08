using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attacks : MonoBehaviour
{
    Player_Handler Handler;

    void Start()
    {
        Player_Handler.Component.anim = GetComponent<Animator>();
        Player_Handler.Component.rb = GetComponent<Rigidbody2D>();
        Handler = GetComponent<Player_Handler>();
    }

    void Update()
    {
        GroundAttack();
    }

    void GroundAttack()
    {
        if (!Player_Handler.State.onGround || !Player_Handler.State.canAttack) return;

        // Basic Attacks
        // Record attackTime, increment basicCount, adjust speed, and play animation
        if (Input.GetMouseButtonDown(0) && Player_Handler.Counter.basicCount <= 2)
        {
            ResetTriggers();
            switch (Player_Handler.Counter.basicCount)
            {
                case 0:
                    Player_Handler.TimeLength.delayTime = Player_Handler.TimeLength.timeShort;
                    
                    Player_Handler.Component.anim.SetTrigger("Basic1");
                    break;
                case 1:
                    Player_Handler.TimeLength.delayTime = Player_Handler.TimeLength.timeMedium;

                    Player_Handler.Component.anim.SetTrigger("Basic2");
                    break;
                case 2:
                    Player_Handler.TimeLength.delayTime = Player_Handler.TimeLength.timeLong;

                    Player_Handler.Component.anim.SetTrigger("Basic3");
                    break;
            }
            Player_Handler.TimeLength.inputTime = Time.time;
            Player_Handler.Counter.basicCount++;
            Player_Handler.State.canMove = false;
            Player_Handler.State.canAttack = false;
            Player_Handler.Component.rb.AddForce(transform.localScale.x * transform.right * 50);
        }
        else if (Input.GetMouseButtonDown(1) && Player_Handler.Counter.basicCount <= 2)
        {
            switch (Player_Handler.Counter.basicCount)
            {
                case 1:
                    Player_Handler.TimeLength.delayTime = Player_Handler.TimeLength.timeMedium;

                    Handler.shake.Shake(Vector3.right * 0.15f);
                    Player_Handler.TimeLength.inputTime = Time.time;
                    //Player_Handler.Counter.basicCount++;
                    Player_Handler.State.canMove = false;
                    Player_Handler.State.canAttack = false;
                    Player_Handler.Component.rb.AddForce(transform.localScale.x * transform.right * -200);


                    Player_Handler.Component.anim.SetTrigger("Fire1");
                    break;
            }

        }
    }
    void ResetTriggers()
    {
        Player_Handler.Component.anim.ResetTrigger("Basic1");
        Player_Handler.Component.anim.ResetTrigger("Basic2");
        Player_Handler.Component.anim.ResetTrigger("Basic3");
    }
}
