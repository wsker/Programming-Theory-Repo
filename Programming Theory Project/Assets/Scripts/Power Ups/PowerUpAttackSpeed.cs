using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAttackSpeed : PowerUp
{
    override public void Initialize()
    {
        base.Initialize();
        AttackSpeed = 0.25f;
        LifeTimeAfterPickup = 2.0f;
    }
}
