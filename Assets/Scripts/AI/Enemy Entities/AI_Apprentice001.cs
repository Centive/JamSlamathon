using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Apprentice001 : AIBase
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
            print(this.gameObject.name + " is dead");
        }
    }
    public void ApplyKnockback(float amountOfForce)
    {
    }
    public IEnumerator ApplySlow(float slowRate, float slowDuration)
    {
        yield return null;
    }
    public IEnumerator ApplyStun(float stunDuration)
    {
        yield return null;
    }

    //
    protected override void UpdateIdle()
    {
    }
    protected override void UpdateMove()
    {
    }
    protected override void UpdateAttack()
    {
    }
    protected override bool FindTarget()
    {
        return true;
    }

}