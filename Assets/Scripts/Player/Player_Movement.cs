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
        public const float JUMP = 200f;
    }

    struct Ground
    {
        public const float THICKNESS_GROUND = 0.2f;
    }
    public Transform checkGround;
    public LayerMask layerGround;

    struct State
    {
        public static bool facingRight = true;
        public static bool onGround = false;
    }

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

        // CHECK IF GROUNDED //////////////////////////////////////////////////////////////////////

        // Retrieve properties for checking when on ground
        State.onGround = Physics2D.OverlapCircle(checkGround.position, Ground.THICKNESS_GROUND, layerGround);
        Component.animator.SetBool("onGround", State.onGround);

        // Checks if on ground
        if (!State.onGround && Component.rigidBody2d.velocity.y == 0)
        {
            State.onGround = true;
        }

        // Horizontal Movement (Running)
        if (Input.GetKey(KeyCode.A))
        {
            // Move player left
            transform.Translate((Vector3.right) * PlayerStat.SPEED * Time.deltaTime);

            State.facingRight = false;

            Vector3 currentScale = transform.localScale;
            currentScale.x = -1;
            transform.localScale = currentScale;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Move player left
            transform.Translate((Vector3.right) * PlayerStat.SPEED * Time.deltaTime);

            State.facingRight = true;

            Vector3 currentScale = transform.localScale;
            currentScale.x = 1;
            transform.localScale = currentScale;
        }
    }

	void Update ()
    {
        // Vertical Movement (Jumping)
        if (Input.GetButtonDown("Jump") && State.onGround)
        {
            Component.rigidBody2d.AddForce(transform.up * PlayerStat.JUMP);
            State.onGround = false;
        }
	}
}
