using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
    public struct Dynamics
    {
        public static float maxSpeed     = 4f;
        public static float maxDashForce = 200f;
        public static float maxForce     = 60f;
        public static float jumpVelocity = 7f;
        public static float dashSpeed    = 9f;
        public static float runForce;
        public static float dashForce;
    }

    public LayerMask layerGround;
    private Player_Effects Effects;


    void Start()
    {
        Player_Handler.Component.anim = GetComponent<Animator>();
        Player_Handler.Component.rb = GetComponent<Rigidbody2D>();

        Effects = GetComponent<Player_Effects>();
    }

    void FixedUpdate()
    {
        // Prevents movement if canMove is false ////////////////////
        if (!Player_Handler.State.canMove) return;

        // Horizontal Movement (Run)
        if (Input.GetButton("MoveLeft"))
        {
            Player_Handler.State.currentScale.x = -1; Run();
        }
        if (Input.GetButton("MoveRight"))
        {
            Player_Handler.State.currentScale.x = 1; Run();
        }
    }

    void Update()
    {
        // Check if on ground
        Player_Handler.State.onGround = Physics2D.OverlapArea(transform.position + new Vector3(0.098f, -0.32f), transform.position + new Vector3(-0.074f, -0.32f), layerGround);
        Player_Handler.Component.anim.SetBool("onGround", Player_Handler.State.onGround);

        // Retrieve animator properties
        Player_Handler.Component.anim.SetFloat("Velocity_Horizontal", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        Player_Handler.Component.anim.SetFloat("Velocity_Vertical", Player_Handler.Component.rb.velocity.y);
        Player_Handler.Component.anim.SetBool("FacingRight", Player_Handler.State.facingRight);

        // Flip sprite
        Player_Handler.State.currentScale = transform.localScale;
        Player_Handler.State.currentScale.x = Player_Handler.State.facingRight ? 1 : -1;
        transform.localScale = Player_Handler.State.currentScale;

        // Aerial drag
        if (Mathf.Abs(Player_Handler.Component.rb.velocity.x) > 0)
        {
            float drag = 0f;
            if (Input.GetButton("MoveLeft") || Input.GetButton("MoveRight"))
                drag = -Player_Handler.Component.rb.velocity.x * 4;
            else
                drag = -Player_Handler.Component.rb.velocity.x * 8;
            Player_Handler.Component.rb.AddForce(transform.right * drag);
        }

        // Dash - Can dash 150ms after an input and cooldown is 500ms.  Not affected by variable "canMove".
        if (Input.GetKeyDown(KeyCode.LeftShift) && Player_Handler.Counter.dashCount == 0 && Time.time >= Player_Handler.TimeLength.inputTime + 0.15f)
        {
            if (Input.GetButton("MoveLeft"))
                Dash(-1);
            else if (Input.GetButton("MoveRight"))
                Dash(1);
        }
        // Jump
        if (Input.GetButtonDown("Jump") && Player_Handler.State.onGround && Time.time >= Player_Handler.TimeLength.inputTime + Player_Handler.TimeLength.timeShort - 0.2f
            || Input.GetButtonDown("Jump") && Player_Handler.State.onGround && Player_Handler.State.dashing)
        {
            if (Input.GetButton("MoveLeft"))
                Jump(-1);
            else if (Input.GetButton("MoveRight"))
                Jump(1);
            else
                Jump(0);
        }

        
    }

    void Run()
    {
        if (!Player_Handler.State.onGround)
        {
            // * Same for moving right *
            // Reset force back to max force.
            // Reduce amount of force to apply as the velocity comes close to the 
            // max velocity only when the velocity is the same direction as the key input.
            // (For more responsive air control.)
            Dynamics.runForce = Dynamics.maxForce;
            if (Player_Handler.State.currentScale.x == -1 && Player_Handler.Component.rb.velocity.x < 0 || Player_Handler.State.currentScale.x == 1 && Player_Handler.Component.rb.velocity.x > 0)
            {
                Dynamics.runForce = (Dynamics.maxSpeed - Mathf.Abs(Player_Handler.Component.rb.velocity.x)) / Dynamics.maxSpeed * Dynamics.maxForce;
            }
        }
        else
        {
            // Reduce amount of force to apply as the velocity comes close to the max velocity
            Dynamics.runForce = (Dynamics.maxSpeed - Mathf.Abs(Player_Handler.Component.rb.velocity.x)) / Dynamics.maxSpeed * Dynamics.maxForce;
        }
        Player_Handler.Component.rb.AddForce(transform.right * Player_Handler.State.currentScale.x * Dynamics.runForce);
        Player_Handler.State.facingRight = (Player_Handler.State.currentScale.x == 1) ? true : false;
    }

    void Jump(int dir)
    {
        if (Player_Handler.State.dashing == true)
            Player_Handler.Component.rb.velocity = new Vector2(dir * (Mathf.Abs(Player_Handler.Component.rb.velocity.x) + Dynamics.dashSpeed), Dynamics.jumpVelocity);
        else
            Player_Handler.Component.rb.velocity = new Vector2(Player_Handler.Component.rb.velocity.x, Dynamics.jumpVelocity);
        Player_Handler.State.onGround = false;
        Player_Handler.State.canMove = true;

        Effects.SpawnDust_Jump();
    }

    void Dash(int dir)
    {
        Player_Handler.TimeLength.inputTime = Time.time;
        Player_Handler.TimeLength.delayTime = Player_Handler.TimeLength.timeShort / 1.5f;
        Player_Handler.Counter.dashCount++;
        Player_Handler.State.canMove = false;
        Player_Handler.State.canAttack = false;
        Player_Handler.State.dashing = true;
        Player_Handler.State.facingRight = (dir == 1) ? true : false;

        float lift = Player_Handler.State.onGround ? 0 : 2;
        Player_Handler.Component.rb.velocity = new Vector2(dir * (Mathf.Abs(Player_Handler.Component.rb.velocity.x) + Dynamics.dashSpeed), Player_Handler.Component.rb.velocity.y + lift);
        Player_Handler.Component.anim.SetTrigger("Dash");
        Player_Handler.Component.anim.SetBool("ignoreAnyState", true);
    }
}