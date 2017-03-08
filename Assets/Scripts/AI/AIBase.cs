using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBase : MonoBehaviour
{

    protected abstract void UpdateIdle();
    protected abstract void UpdateMove();
    protected abstract void UpdateAttack();
    protected abstract bool FindTarget();

    public enum Estate
    {
        Enone,
        Eidle,
        Emove,
        Eattack
    }

    public Estate curState = Estate.Eidle;
    public float maxSpeed = 0f;
    public float curSpeed = 0f;
    public int maxHealth = 0;
    public int curHealth = 0;
    public bool isFacingRight = true;
    public Vector3 currentScale;
    public Transform target;

    //Updates:
    // AI's behaviour
    void Update()
    {
        //Debug.Log(isFacingRight);

        if (target)
        {
            if ((transform.InverseTransformPoint(target.position).x > 0f))
            {
                isFacingRight = true;
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                isFacingRight = false;
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            currentScale = transform.localScale;
            currentScale.x = (isFacingRight) ? 1f : -1f;
            transform.localScale = currentScale;
        }

        CheckBehaviour();
    }
    
    //Override these functions if an AI has a different logic
    protected virtual void CheckBehaviour()
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
}