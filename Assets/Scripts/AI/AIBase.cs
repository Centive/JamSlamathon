using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBase : MonoBehaviour
{
    protected abstract void UpdateIdle();
    protected abstract void UpdateMove();
    protected abstract void UpdateAttack();

    public enum Estate
    {
        Eidle,
        Emove,
        Eattack
    }

    public Estate curState = Estate.Eidle;
    public float maxSpeed = 0f;
    public float curSpeed = 0f;

    //Updates:
    // AI's behaviour
    void Update()
    {
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
    protected virtual void FindPlayer()
    {
        //Nothing for now
    }
}
