using UnityEngine;
using System.Collections;

public interface IDamageable<T>
{
    T health { get; set; }
    void ApplyDamage(T damageDone);
}

public interface IKnockback<T>
{
    void ApplyKnockback(T amountOfForce);
}

public interface ISlow<T>
{
    IEnumerator ApplySlow(T slowRate, T slowDuration);
}

public interface IStun<T>
{
    IEnumerator ApplyStun(T stunDuration);
}

public interface IItem<T>
{
    T value { get; set; }
    void Use();
    void Sell();
}

public interface IQuest<T>
{
    void Accept();
    void HandIn();
}