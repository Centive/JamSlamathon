using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Effects : MonoBehaviour
{
    public GameObject Feet;
    private Animator anim;

    // Effect Prefabs
    private GameObject Dust;

    void Start()
    {
        Dust = Resources.Load("Prefabs/Effects/Effects_Dust") as GameObject;
    }

    // Run
    void SpawnDust_Run()
    {
        int dir = (Player_Handler.State.facingRight) ? 0 : 180;
        GameObject clone = Instantiate(Dust, Feet.transform.position, Quaternion.Euler(0, dir, 0)) as GameObject;
        anim = clone.GetComponent<Animator>();

        anim.SetTrigger("Run");
        Destroy(clone, 1f);
    }
    // Jump
    public void SpawnDust_Jump()
    {
        int dir = (Player_Handler.State.facingRight) ? 0 : 180;
        GameObject clone = Instantiate(Dust, Feet.transform.position, Quaternion.Euler(0, dir, 0)) as GameObject;
        anim = clone.GetComponent<Animator>();

        anim.SetTrigger("Jump");
        Destroy(clone, 1f);
    }
    // Land
    void SpawnDust_Land()
    {
        int dir = (Player_Handler.State.facingRight) ? 0 : 180;
        GameObject clone = Instantiate(Dust, Feet.transform.position, Quaternion.Euler(0, dir, 0)) as GameObject;
        anim = clone.GetComponent<Animator>();

        anim.SetTrigger("Land");
        Destroy(clone, 1f);
    }
    // Dash
    public void SpawnDust_Dash()
    {
        if (!Player_Handler.State.onGround) return;
        int dir = (Player_Handler.State.facingRight) ? 0 : 180;
        GameObject clone = Instantiate(Dust, Feet.transform.position, Quaternion.Euler(0, dir, 0)) as GameObject;
        anim = clone.GetComponent<Animator>();

        anim.SetTrigger("Dash");
        Destroy(clone, 1f);
    }
}
