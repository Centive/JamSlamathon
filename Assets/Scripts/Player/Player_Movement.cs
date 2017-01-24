using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
    struct Component
    {
        public static Animator animator;
        public static Rigidbody2D rigidBody2d;
    }
    struct PlayerStat
    {
        public const float SPEED = 4f;
        public const float JUMP = 5f;
    }
    struct Ground
    {
        public const float THICKNESS_GROUND = 0.2f;
    }
    struct State
    {
        public static bool facingRight = true;
        public static bool onGround = false;
    }

    public Transform checkGround;
    public LayerMask layerGround;

    void Start ()
    {
        Component.animator = GetComponent<Animator>();
        Component.rigidBody2d = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate()
    {
        // Retrieve horizontal movement
        float movement = Input.GetAxisRaw("Horizontal");

        // Retrieve animator properties
        Component.animator.SetFloat("Velocity_Horizontal", Mathf.Abs(movement));
        Component.animator.SetFloat("Velocity_Vertical", Component.rigidBody2d.velocity.y);
        Component.animator.SetBool("FacingRight", State.facingRight);

        // CHECK IF GROUNDED /////////////////////////////////////////////////
        /////////////////////

        // Retrieve properties for checking when on ground
        State.onGround = Physics2D.OverlapCircle(checkGround.position, Ground.THICKNESS_GROUND, layerGround);
        Component.animator.SetBool("onGround", State.onGround);

        // Checks if on ground
        if (!State.onGround && Component.rigidBody2d.velocity.y == 0)
        {
            State.onGround = true;
        }

        //Flip sprite
        Vector3 currentScale = transform.localScale;
        currentScale.x = (State.facingRight) ? 1f : -1f;
        transform.localScale = currentScale;
        //end flip sprite

        // Horizontal Movement (Running)
        if (Input.GetButton("MoveLeft"))
        {
            // Move player left
            Component.rigidBody2d.velocity = new Vector2(-PlayerStat.SPEED, Component.rigidBody2d.velocity.y);
            State.facingRight = false;
        }
        else if (Input.GetButton("MoveRight"))
        {
            // Move player right
            Component.rigidBody2d.velocity = new Vector2(PlayerStat.SPEED, Component.rigidBody2d.velocity.y);
            State.facingRight = true;
        }

    }

    void Update ()
    {
        // Vertical Movement (Jumping)
        if (Input.GetButtonDown("Jump") && State.onGround)
        {
            Component.rigidBody2d.velocity = new Vector2(Component.rigidBody2d.velocity.x, PlayerStat.JUMP);
            Component.rigidBody2d.AddForce(transform.up * PlayerStat.JUMP);
            State.onGround = false;
        }
	}
}
