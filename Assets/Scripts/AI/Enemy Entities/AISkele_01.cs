using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkele_01 : AIBase
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
        if (health <= 0)
        {
            print("Player is dead");
        }
    }
    public void ApplyKnockback(float amountOfForce)
    {
       myRb.AddForce(-(new Vector2(currentScale.x, -currentScale.y)) * amountOfForce);
    }
    public IEnumerator ApplySlow(float slowRate, float slowDuration)
    {
        float time = 0f;
        while (slowDuration > time && Mathf.Abs(myRb.velocity.x) >= 0f)
        {
            time += Time.deltaTime;
            float drag = -myRb.velocity.x * slowRate;
            if(curState == Estate.Emove)
                myRb.AddForce(transform.right * drag);
            yield return null;
        }
    }
    public IEnumerator ApplyStun(float stunDuration)
    {
        float time = 0f;
        while (stunDuration > time)
        {
            time += Time.deltaTime;
            curState = Estate.Enone;
            yield return null;
        }
        curState = Estate.Eattack;
    }
    //

    //vars
    public float maxMoveTime = 0f;
    public float curMoveTime = 0f;
    public float maxIdleTime = 0f;
    public float curIdleTime = 0f;
    public GameObject prefabProjectile;

    //placeholders
    public Sprite Sattacking;
    public Sprite SidleOrMove;

    //Firing bullets
    public float fireRate = 0f;
    public float nextFire = 0f;


    //test for ai attacking
    public float aggroDist = 0f;
    public Rect aggroBox = Rect.zero;
    public Vector2 aggroDir = Vector2.zero;
    public Vector2 aggroOrigin = Vector2.zero;

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
        health = 50;
    }

    //Update states
    protected override void UpdateIdle()
    {
        //Check if the target is near
        if (FindTarget())
        {
            curState = Estate.Eattack;
        }

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
        //Check if the target is near
        if (FindTarget())
        {
            curState = Estate.Eattack;
        }

        //Play move state...

        //Move ai
        if (!isFacingRight)
        {
            //move timer
            curMoveTime -= Time.deltaTime;
            if (curMoveTime < 0f)
            {
                curState = Estate.Eidle;

                //moves the ai the other way next time
                isFacingRight = true;
                curMoveTime = maxMoveTime;
            }
            else
            {
                //move the ai
                myRb.velocity = new Vector2(-curSpeed, myRb.velocity.y);
            }
        }
        if (isFacingRight)
        {
            //move timer
            curMoveTime -= Time.deltaTime;
            if (curMoveTime < 0f)
            {
                curState = Estate.Eidle;

                //moves the ai the other way next time
                isFacingRight = false;
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
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            target = null;
            curState = Estate.Eidle;
        }
        
        //isFacingRight = () ? true : false;
        
        //Attack the player
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Vector3 bulletPos = new Vector3(0.302f, 0.333f);
            if (!isFacingRight) { bulletPos.x = -bulletPos.x; } 

            //Fire bullet
            GameObject bullet = Instantiate(prefabProjectile, transform.position + bulletPos, Quaternion.identity) as GameObject;
            if(target)
                bullet.GetComponent<ProjectileBase>().bulletDir = (target.position - transform.position).normalized;
        }
    }
    //Overriding the virtual function
    protected override bool FindTarget()
    {
        //Change the boxcast's dir, and origin depending where
        //the player is facing
        if(isFacingRight)
        {
            aggroOrigin = (Vector2)transform.position + aggroBox.center;
            aggroDir = Vector2.right;
        }
        else if(!isFacingRight)
        {
            aggroOrigin = (Vector2)transform.position - aggroBox.center;
            aggroDir = Vector2.left;
        }

        RaycastHit2D hit = Physics2D.BoxCast(
            aggroOrigin, 
            aggroBox.size, transform.eulerAngles.z,
            aggroDir, 
            aggroDist, 
            mask);
        

        //Check for hits
        if (hit.collider != null)
        {
            //Check if it's the player
            if (hit.collider.gameObject.tag == "Player")
            {
                target = hit.transform;
                return true;
            }
        }

        return false;
    }

    //Draw boxcast
    void OnDrawGizmos()
    {
        //Draw cubes
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(aggroOrigin, this.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, aggroBox.size);
        Gizmos.matrix = Matrix4x4.TRS(aggroOrigin + (aggroDir.normalized * aggroDist), this.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector2.zero, aggroBox.size);
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(aggroOrigin, Quaternion.identity, Vector3.one);
        Gizmos.DrawLine(Vector2.zero, aggroDir.normalized * aggroDist);

    }
}