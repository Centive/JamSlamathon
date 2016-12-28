using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRb;
    public float mMaxSpeed;
    public float mCurSpeed;

    //Debugging stuff
    public bool killPlayer = false;

    // Use this for initialization
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        mCurSpeed = mMaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = (Input.GetAxis("Horizontal") * mCurSpeed);

        myRb.velocity = new Vector2(xInput, myRb.velocity.y);

        //Enable debugging
        if (killPlayer) { Destroy(this.gameObject); }
    }
    
    //Debugging funcs
    public void ToggleKill()
    {
        killPlayer = true;
    }
}
