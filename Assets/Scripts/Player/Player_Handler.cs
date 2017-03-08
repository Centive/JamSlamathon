using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Handler : MonoBehaviour
    , IDamageable<int>
    , IKnockback<float>
    , ISlow<float>
    , IStun<float>
{
    //interfaces
    public int health { get; set; }
    public void ApplyDamage(int damageDone)
    {
        health -= damageDone;
        if(health <= 0)
        {
            print("Player is dead");
        }
    }
    public void ApplyKnockback(float amountOfForce)
    {
        print(State.currentScale);
        Component.rb.AddForce( -(new Vector2(State.currentScale.x, 0f)) * amountOfForce);
    }
    public IEnumerator ApplySlow(float slowRate, float slowDuration)
    {
        float time = 0f;
        while (slowDuration > time && Mathf.Abs(Component.rb.velocity.x) >= 0f)
        {
            print("slowed: " + time);
            time += Time.deltaTime;
            float drag = -Component.rb.velocity.x * slowRate;
            Component.rb.AddForce(transform.right * drag);
            yield return null;
        }
    }
    public IEnumerator ApplyStun(float stunDuration)
    {
        float time = 0f;
        while(stunDuration > time)
        {
            print("stunned: " + time);
            time += Time.deltaTime;
            State.canMove = false;
            yield return null;
        }
        State.canMove = true;
    }

    //
    public struct Component
    {
        public static Animator anim;
        public static Rigidbody2D rb;
    }
    public struct State
    {
        public static bool facingRight = true;
        public static bool onGround    = false;
        public static bool canMove     = true;
        public static bool canAttack   = true;
        public static bool dashing     = false;
        public static Vector3 currentScale;
    }
    public struct TimeLength
    {
        public static float timeShort  = 0.32f;
        public static float timeMedium = 0.40f;
        public static float timeLong   = 0.44f;
        public static float inputTime  = 0f;
        public static float delayTime  = 0f;
    }
    public struct Counter
    {
        public static int basicCount = 0;
        public static int dashCount  = 0;
    }

    // Access ScreenShake script
    [HideInInspector]
    public ScreenShake shake;

    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();
    }

    void Update()
    {
        // Reenable Movement and reset attacks after given delay
        if (Time.time >= TimeLength.inputTime + TimeLength.delayTime)
        {
            State.canMove = true;
            Counter.basicCount = 0;
            Counter.dashCount = 0;
            State.dashing = false;
        }
        if (Time.time >= TimeLength.inputTime + TimeLength.delayTime / 3.5f)
        {
            State.canAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            print("pressed");
            StartCoroutine(ApplySlow(20f, 1f));
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            print("pressed");
            StartCoroutine(ApplyStun(5f));
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ApplyKnockback(500f);
        }
    }
}